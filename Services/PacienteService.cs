using Microsoft.Data.Sqlite;
using VirchowAspNetApi.Models;

namespace VirchowAspNetApi.Services;

    public class PacienteService
    {
        private readonly string _connectionString = "Data Source=virchow.db";

        public PacienteService()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Paciente (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                Flg_sexo TEXT,
                Dat_nascimento DATE,
                Estado_civil INTEGER,
                FOREIGN KEY (Estado_civil) REFERENCES EstadoCivil(Id)
            );";
            tableCmd.ExecuteNonQuery();
        }

        public List<Paciente> GetAll()
        {
            var pacientes = new List<Paciente>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome, Flg_sexo, Dat_nascimento, Estado_civil, (SELECT Descricao FROM EstadoCivil WHERE ID = Estado_civil) FROM Paciente";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pacientes.Add(new Paciente
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Sexo = reader.GetString(2),
                    DatNascimento = Convert.ToDateTime(reader.GetString(3)),
                    EstadoCivil = new EstadoCivil
                    {
                        Id = reader.GetInt32(4),
                        Descricao = reader.GetString(5)
                    }
                });
            }

            return pacientes;
        }

        public Paciente? GetById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome, Flg_sexo, Dat_nascimento, Estado_civil, (SELECT Descricao FROM EstadoCivil WHERE ID = Estado_civil) FROM Paciente WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Paciente
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Sexo = reader.GetString(2),
                    DatNascimento = Convert.ToDateTime(reader.GetString(3)),
                    EstadoCivil = new EstadoCivil
                    {
                        Id = reader.GetInt32(4),
                        Descricao = reader.GetString(5)
                    }
                };
            }

            return null;
        }

        public Paciente Add(Paciente paciente)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Paciente (Nome, Flg_sexo, Dat_nascimento, Estado_civil) VALUES ($nome, $flg_sexo, $dat_nascimento, $estado_civil); SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$nome", paciente.Nome);
            cmd.Parameters.AddWithValue("$flg_sexo", paciente.Sexo);
            cmd.Parameters.AddWithValue("$dat_nascimento", paciente.DatNascimento);
            cmd.Parameters.AddWithValue("$estado_civil", paciente.EstadoCivil.Id);            

            paciente.Id = (int)(long)cmd.ExecuteScalar()!;
            return paciente;
        }

        public bool Update(int id, Paciente paciente)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Paciente SET Nome = $nome, Flg_sexo = $flg_sexo, Dat_nascimento = $dat_nascimento, Estado_civil = $estado_civil WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.Parameters.AddWithValue("$nome", paciente.Nome);
            cmd.Parameters.AddWithValue("$flg_sexo", paciente.Sexo);
            cmd.Parameters.AddWithValue("$dat_nascimento", paciente.DatNascimento);
            cmd.Parameters.AddWithValue("$estado_civil", paciente.EstadoCivil.Id);   

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Paciente WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
}
