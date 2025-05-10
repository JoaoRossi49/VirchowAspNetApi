using Microsoft.Data.Sqlite;
using VirchowAspNetApi.Models;

namespace VirchowAspNetApi.Services;

    public class DiagnosticoService
    {
        private readonly string _connectionString = "Data Source=virchow.db";

        public DiagnosticoService()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Diagnostico (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ExameId INTEGER NOT NULL,
                Codigo TEXT NOT NULL,
                Conteudo TEXT NOT NULL,
                Dat_fim DATE
            );";
            tableCmd.ExecuteNonQuery();
        }

        public List<Diagnostico> GetByExameId(int exameId)
        {
            var Diagnosticos = new List<Diagnostico>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM Diagnostico WHERE Dat_fim is null AND ExameId = {exameId}";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Diagnosticos.Add(new Diagnostico
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                    Conteudo = reader.GetString(reader.GetOrdinal("Conteudo"))
                });
            }

            return Diagnosticos;
        }

}
