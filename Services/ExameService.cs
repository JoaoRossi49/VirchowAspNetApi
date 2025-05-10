using Microsoft.Data.Sqlite;
using VirchowAspNetApi.Models;

namespace VirchowAspNetApi.Services;

public class ExameService
{
    private readonly string _connectionString = "Data Source=virchow.db";

    public ExameService()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Exame (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                Dat_fim DATE
            );";
        tableCmd.ExecuteNonQuery();
    }

    public List<Exame> GetAll()
    {
        var tipos = new List<Exame>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Exame WHERE Dat_fim is null";
        using var reader = cmd.ExecuteReader();

        TopicoService TopicoService = new TopicoService();
        while (reader.Read())
        {
            tipos.Add(new Exame
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                TopicosList = TopicoService.GetByExameId(reader.GetInt32(reader.GetOrdinal("Id")))
            });
        }

        return tipos;
    }

    public Exame GetById(int id)
    {
        var tipos = new List<Exame>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT * FROM Exame WHERE Dat_fim is null AND ID = {id}";

        using var reader = cmd.ExecuteReader();
        TopicoService TopicoService = new TopicoService();
        while (reader.Read())
        {
            return (new Exame
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                TopicosList = TopicoService.GetByExameId(reader.GetInt32(reader.GetOrdinal("Id")))
            });
        }

        return null ;
    }

}
