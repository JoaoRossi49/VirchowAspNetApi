using Microsoft.Data.Sqlite;
using VirchowAspNetApi.Models;

namespace VirchowAspNetApi.Services;

    public class TipoLaudoService
    {
        private readonly string _connectionString = "Data Source=virchow.db";

        public TipoLaudoService()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS TipoLaudo (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                Grupo TEXT NOT NULL, 
                Conteudo TEXT NOT NULL,
                Dat_fim DATE
            );";
            tableCmd.ExecuteNonQuery();
        }

        public List<TipoLaudo> GetAll()
        {
            var tipos = new List<TipoLaudo>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM TipoLaudo WHERE Dat_fim is null";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tipos.Add(new TipoLaudo
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Grupo = reader.GetString(2),
                    Conteudo = reader.GetString(3)
                });
            }

            return tipos;
        }

}
