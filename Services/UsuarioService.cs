using System.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Data.Sqlite;
using VirchowAspNetApi.Models;

namespace VirchowAspNetApi.Services;

    public class UsuarioService
    {
        private readonly string _connectionString = "Data Source=virchow.db";

        public UsuarioService()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Usuario (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                Login TEXT NOT NULL,
                Senha TEXT NOT NULL,
                Dat_fim DATE
            );";
            tableCmd.ExecuteNonQuery();
        }

        public Usuario GetByLogin(string Login)
        {
            var usuario = new Usuario();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Usuario WHERE Login = $login AND Dat_fim is null";
            cmd.Parameters.AddWithValue("$login", Login);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Login = reader.GetString(2),
                    Senha = reader.GetString(3)
                };
            }

            return null;
        }

}
