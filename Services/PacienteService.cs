﻿using Microsoft.Data.Sqlite;
using VirchowAspNetApi.Models;

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

    public List<Paciente> GetAll()
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
            LEFT JOIN EstadoCivil ec ON ec.Id = p.Estado_civil";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            pacientes.Add(new Paciente
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Sexo = reader.IsDBNull(2) ? null : reader.GetString(2),
                DatNascimento = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                EstadoCivil = reader.IsDBNull(4) ? null : new EstadoCivil
                {
                    Id = reader.GetInt32(4),
                    Descricao = reader.IsDBNull(5) ? null : reader.GetString(5)
                },
                Profissao = reader.IsDBNull(6) ? null : reader.GetString(6),
                Procedencia = reader.IsDBNull(7) ? null : reader.GetString(7)
            });
        }

        return pacientes;
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
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Sexo = reader.IsDBNull(2) ? null : reader.GetString(2),
                DatNascimento = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                EstadoCivil = reader.IsDBNull(4) ? null : new EstadoCivil
                {
                    Id = reader.GetInt32(4),
                    Descricao = reader.IsDBNull(5) ? null : reader.GetString(5)
                },
                Profissao = reader.IsDBNull(6) ? null : reader.GetString(6),
                Procedencia = reader.IsDBNull(7) ? null : reader.GetString(7)
            };
        }

        return null;
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
