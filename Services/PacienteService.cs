using Microsoft.Data.Sqlite;
using VirchowAspNetApi.Models;
using VirchowAspNetApi.DTOs;

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
            Profissao TEXT,
            Procedencia TEXT,
            FOREIGN KEY (Estado_civil) REFERENCES EstadoCivil(Id)
        );";
        tableCmd.ExecuteNonQuery();
    }

    public Paciente? GetById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT 
                p.Id, p.Nome, p.Flg_sexo, p.Dat_nascimento, 
                p.Estado_civil, ec.Descricao, p.Profissao, p.Procedencia
            FROM Paciente p
            LEFT JOIN EstadoCivil ec ON ec.Id = p.Estado_civil
            WHERE p.Id = $id";
        cmd.Parameters.AddWithValue("$id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Paciente
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                Sexo = reader.IsDBNull(reader.GetOrdinal("Flg_sexo")) ? null : reader.GetString(reader.GetOrdinal("Flg_sexo")),
                DatNascimento = reader.IsDBNull(reader.GetOrdinal("Dat_nascimento")) ? null : reader.GetDateTime(reader.GetOrdinal("Dat_nascimento")),
                EstadoCivil = reader.IsDBNull(reader.GetOrdinal("Estado_civil")) ? null : new EstadoCivil
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Estado_civil")),
                    Descricao = reader.IsDBNull(reader.GetOrdinal("Descricao")) ? null : reader.GetString(reader.GetOrdinal("Descricao"))
                },
                Profissao = reader.IsDBNull(reader.GetOrdinal("Profissao")) ? null : reader.GetString(reader.GetOrdinal("Profissao")),
                Procedencia = reader.IsDBNull(reader.GetOrdinal("Procedencia")) ? null : reader.GetString(reader.GetOrdinal("Procedencia"))
            };
        }

        return null;
    }

    public List<Paciente> GetByFilter(PacienteFilter paciente)
    {
        var pacientes = new List<Paciente>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT 
                p.Id, p.Nome, p.Flg_sexo, p.Dat_nascimento, 
                p.Estado_civil, ec.Descricao, p.Profissao, p.Procedencia
            FROM Paciente p
            LEFT JOIN EstadoCivil ec ON ec.Id = p.Estado_civil
            WHERE 1 = 1 " +
            (!String.IsNullOrEmpty(paciente.Nome) ? $"AND p.Nome like '%{paciente.Nome}%'" : "") +
            (paciente.DatNascimento != null ? $"AND p.Dat_nascimento = '{paciente.DatNascimento:yyyy-MM-dd HH:mm:ss}'" : "");


        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            pacientes.Add(new Paciente
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                Sexo = reader.IsDBNull(reader.GetOrdinal("Flg_sexo")) ? null : reader.GetString(reader.GetOrdinal("Flg_sexo")),
                DatNascimento = reader.IsDBNull(reader.GetOrdinal("Dat_nascimento")) ? null : reader.GetDateTime(reader.GetOrdinal("Dat_nascimento")),
                EstadoCivil = reader.IsDBNull(reader.GetOrdinal("Estado_civil")) ? null : new EstadoCivil
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Estado_civil")),
                    Descricao = reader.IsDBNull(reader.GetOrdinal("Descricao")) ? null : reader.GetString(reader.GetOrdinal("Descricao"))
                },
                Profissao = reader.IsDBNull(reader.GetOrdinal("Profissao")) ? null : reader.GetString(reader.GetOrdinal("Profissao")),
                Procedencia = reader.IsDBNull(reader.GetOrdinal("Procedencia")) ? null : reader.GetString(reader.GetOrdinal("Procedencia"))

            });
        }

        return pacientes;
    }

    public Paciente Add(Paciente paciente)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Paciente 
            (Nome, Flg_sexo, Dat_nascimento, Estado_civil, Profissao, Procedencia) 
            VALUES 
            ($nome, $flg_sexo, $dat_nascimento, $estado_civil, $profissao, $procedencia);
            SELECT last_insert_rowid();";

        cmd.Parameters.AddWithValue("$nome", paciente.Nome);
        cmd.Parameters.AddWithValue("$flg_sexo", (object?)paciente.Sexo ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$dat_nascimento", (object?)paciente.DatNascimento ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$estado_civil", paciente.EstadoCivil?.Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$profissao", (object?)paciente.Profissao ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$procedencia", (object?)paciente.Procedencia ?? DBNull.Value);

        paciente.Id = Convert.ToInt32((long)cmd.ExecuteScalar()!);
        return paciente;
    }

    public bool Update(int id, Paciente paciente)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            UPDATE Paciente 
            SET Nome = $nome, Flg_sexo = $flg_sexo, Dat_nascimento = $dat_nascimento, 
                Estado_civil = $estado_civil, Profissao = $profissao, Procedencia = $procedencia 
            WHERE Id = $id";

        cmd.Parameters.AddWithValue("$id", id);
        cmd.Parameters.AddWithValue("$nome", paciente.Nome);
        cmd.Parameters.AddWithValue("$flg_sexo", (object?)paciente.Sexo ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$dat_nascimento", (object?)paciente.DatNascimento ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$estado_civil", paciente.EstadoCivil?.Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$profissao", (object?)paciente.Profissao ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$procedencia", (object?)paciente.Procedencia ?? DBNull.Value);

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
