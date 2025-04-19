using Microsoft.Data.Sqlite;
using VirchowAspNetApi.Models;

namespace VirchowAspNetApi.Services;

    public class MascaraService
    {
        private readonly string _connectionString = "Data Source=virchow.db";

        public MascaraService()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Mascara (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                Conteudo TEXT NOT NULL,
                Dat_fim DATE
            );";
            tableCmd.ExecuteNonQuery();
        }

        public List<Mascara> GetAll()
        {
            var mascaras = new List<Mascara>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Mascara WHERE Dat_fim is null";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                mascaras.Add(new Mascara
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Conteudo = reader.GetString(2)
                });
            }

            return mascaras;
        }

}
