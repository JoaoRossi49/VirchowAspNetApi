using Microsoft.Data.Sqlite;
using VirchowAspNetApi.Models;

namespace VirchowAspNetApi.Services;

    public class TopicoService
    {
        private readonly string _connectionString = "Data Source=virchow.db";

        public TopicoService()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Topico (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ExameId INT NOT NULL,
                Conteudo TEXT NOT NULL,
                Dat_fim DATE
            );";
            tableCmd.ExecuteNonQuery();
        }

        public List<String> GetByExameId(int ExameId)
        {
            var Topicos = new List<String>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM Topico WHERE Dat_fim is null AND ExameId = {ExameId}";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Topicos.Add(reader.GetString(reader.GetOrdinal("Conteudo")));
            }

            return Topicos;
        }

}
