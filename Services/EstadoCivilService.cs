using Microsoft.Data.Sqlite;
using VirchowAspNetApi.Models;

namespace VirchowAspNetApi.Services;

    public class EstadoCivilService
    {
        private readonly string _connectionString = "Data Source=virchow.db";

        public EstadoCivilService()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS EstadoCivil (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Descricao TEXT NOT NULL,
                Dat_fim DATE
            );";
            tableCmd.ExecuteNonQuery();
        }

        public List<EstadoCivil> GetAll()
        {
            var produtos = new List<EstadoCivil>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Descricao FROM EstadoCivil WHERE Dat_fim is null";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                produtos.Add(new EstadoCivil
                {
                    Id = reader.GetInt32(0),
                    Descricao = reader.GetString(1)
                });
            }

            return produtos;
        }

}
