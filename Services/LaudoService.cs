using Microsoft.Data.Sqlite;
using VirchowAspNetApi.DTOs;
using VirchowAspNetApi.Models;

namespace VirchowAspNetApi.Services
{
    public class LaudoService
    {
        private readonly string _connectionString = "Data Source=virchow.db";

        public LaudoService()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Laudo (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NroLaudo INTEGER,
                    NomePaciente TEXT NOT NULL,
                    Idade TEXT, 
                    EstadoCivil TEXT,
                    ResumoClinico TEXT,
                    HipoteseDiagnostica TEXT,
                    DatUltimaMenstruacao DATE,
                    MedicoRequisitante TEXT,
                    DatExame DATE,
                    DatInclusao DATE,
                    DatImpressao DATE,
                    DesLaudo TEXT,
                    DatInvalidado DATE, 
                    Usuario_invalida_id INTEGER,
                    TipoLaudoId INTEGER,
                    Laudo_complementar_id INTEGER
                );";
            tableCmd.ExecuteNonQuery();
        }

        public List<Laudo> GetAll()
        {
            var laudos = new List<Laudo>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Laudo WHERE DatInvalidado is null";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                TipoLaudoService tipoLaudoService = new TipoLaudoService();
                laudos.Add(new Laudo
                {
                    Id = reader.GetInt32(0),
                    NroLaudo = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                    NomePaciente = reader.GetString(2),
                    Idade = reader.IsDBNull(3) ? null : reader.GetString(3),
                    EstadoCivil = reader.IsDBNull(4) ? null : reader.GetString(4),
                    ResumoClinico = reader.IsDBNull(5) ? null : reader.GetString(5),
                    HipoteseDiagnostica = reader.IsDBNull(6) ? null : reader.GetString(6),
                    DatUltimaMenstruacao = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                    MedicoRequisitante = reader.IsDBNull(8) ? null : reader.GetString(8),
                    DatExame = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                    DatInclusao = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                    DatImpressao = reader.IsDBNull(11) ? null : reader.GetDateTime(11),
                    DesLaudo = reader.IsDBNull(12) ? null : reader.GetString(12),
                    DatInvalidado = reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                    UsuarioInvalidaId = reader.IsDBNull(14) ? null : reader.GetInt32(14),
                    TipoLaudo = tipoLaudoService.GetById(reader.IsDBNull(15) ? 0 : reader.GetInt32(15))
                });
            }

            return laudos;
        }

        public Laudo? GetById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Laudo WHERE Id = $id AND DatInvalidado is null";
            cmd.Parameters.AddWithValue("$id", id);

            TipoLaudoService tipoLaudoService = new TipoLaudoService();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Laudo
                {
                    Id = reader.GetInt32(0),
                    NroLaudo = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                    NomePaciente = reader.GetString(2),
                    Idade = reader.IsDBNull(3) ? null : reader.GetString(3),
                    EstadoCivil = reader.IsDBNull(4) ? null : reader.GetString(4),
                    ResumoClinico = reader.IsDBNull(5) ? null : reader.GetString(5),
                    HipoteseDiagnostica = reader.IsDBNull(6) ? null : reader.GetString(6),
                    DatUltimaMenstruacao = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                    MedicoRequisitante = reader.IsDBNull(8) ? null : reader.GetString(8),
                    DatExame = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                    DatInclusao = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                    DatImpressao = reader.IsDBNull(11) ? null : reader.GetDateTime(11),
                    DesLaudo = reader.IsDBNull(12) ? null : reader.GetString(12),
                    DatInvalidado = reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                    UsuarioInvalidaId = reader.IsDBNull(14) ? null : reader.GetInt32(14),
                    TipoLaudo = tipoLaudoService.GetById(reader.IsDBNull(15) ? 0 : reader.GetInt32(15))
                };
            }

            return null;
        }

        public LaudoRequest Add(LaudoRequest laudo)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Laudo 
                (NomePaciente, Idade, EstadoCivil, ResumoClinico, HipoteseDiagnostica, DatUltimaMenstruacao, MedicoRequisitante, DatExame, DatInclusao, DatImpressao, DesLaudo, TipoLaudoId)
                VALUES 
                ($nomePaciente, $idade, $estadoCivil, $resumoClinico, $hipotese, $datUltMenstr, $medico, $datExame, DATETIME('now', 'localtime'), DATETIME('now', 'localtime'), $desLaudo, $tipoLaudoId);
                SELECT last_insert_rowid();";

            cmd.Parameters.AddWithValue("$nomePaciente", laudo.NomePaciente);
            cmd.Parameters.AddWithValue("$idade", laudo.Idade ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$estadoCivil", laudo.EstadoCivil ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$resumoClinico", laudo.ResumoClinico ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$hipotese", laudo.HipoteseDiagnostica ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datUltMenstr", laudo.DatUltimaMenstruacao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$medico", laudo.MedicoRequisitante ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datExame", laudo.DatExame ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$desLaudo", laudo.DesLaudo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$tipoLaudoId", laudo.TipoLaudoId > 0 ? laudo.TipoLaudoId : (object)DBNull.Value);


            laudo.Id = (int)(long)cmd.ExecuteScalar()!;
            return laudo;
        }

        public LaudoComplementarRequest Add(LaudoComplementarRequest laudo)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Laudo 
                (NomePaciente, Idade, EstadoCivil, ResumoClinico, HipoteseDiagnostica, DatUltimaMenstruacao, MedicoRequisitante, DatExame, DatInclusao, DatImpressao, DesLaudo, Laudo_complementar_id)
                VALUES 
                ($nomePaciente, $idade, $estadoCivil, $resumoClinico, $hipotese, $datUltMenstr, $medico, $datExame, DATETIME('now', 'localtime'), DATETIME('now', 'localtime'), $desLaudo, $tipoLaudoId, $laudoComplementarId);
                SELECT last_insert_rowid();";

            cmd.Parameters.AddWithValue("$nomePaciente", laudo.NomePaciente);
            cmd.Parameters.AddWithValue("$idade", laudo.Idade ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$estadoCivil", laudo.EstadoCivil ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$resumoClinico", laudo.ResumoClinico ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$hipotese", laudo.HipoteseDiagnostica ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datUltMenstr", laudo.DatUltimaMenstruacao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$medico", laudo.MedicoRequisitante ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datExame", laudo.DatExame ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$desLaudo", laudo.DesLaudo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$tipoLaudoId", laudo.TipoLaudoId > 0 ? laudo.TipoLaudoId : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$laudoComplementarId", laudo.LaudoComplementarId);


            laudo.Id = (int)(long)cmd.ExecuteScalar()!;
            return laudo;
        }

        public bool Update(int id, Laudo laudo)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE Laudo SET 
                    NomePaciente = $nomePaciente,
                    Idade = $idade,
                    EstadoCivil = $estadoCivil,
                    ResumoClinico = $resumoClinico,
                    HipoteseDiagnostica = $hipotese,
                    DatUltimaMenstruacao = $datUltMenstr,
                    MedicoRequisitante = $medico,
                    DatExame = $datExame,
                    DatImpressao = $datImpressao,
                    DesLaudo = $desLaudo,
                    DatInvalidado = $datInvalidado,
                    TipoLaudoId = $tipoLaudoId,
                    Usuario_invalida_id = $usuarioInvalidaId
                WHERE Id = $id";

            cmd.Parameters.AddWithValue("$id", id);
            cmd.Parameters.AddWithValue("$nomePaciente", laudo.NomePaciente);
            cmd.Parameters.AddWithValue("$idade", laudo.Idade ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$estadoCivil", laudo.EstadoCivil ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$resumoClinico", laudo.ResumoClinico ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$hipotese", laudo.HipoteseDiagnostica ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datUltMenstr", laudo.DatUltimaMenstruacao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$medico", laudo.MedicoRequisitante ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datExame", laudo.DatExame ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datImpressao", laudo.DatImpressao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$desLaudo", laudo.DesLaudo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datInvalidado", laudo.DatInvalidado ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$tipoLaudoId", laudo.TipoLaudo.Id > 0 ? laudo.TipoLaudo.Id : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$usuarioInvalidaId", laudo.UsuarioInvalidaId ?? (object)DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Invalidate(int id, int usuarioId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE LAUDO SET Usuario_invalida_id = $usuarioId, DatInvalidado = DATETIME('now', 'localtime') WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.Parameters.AddWithValue("$usuarioId", usuarioId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Complement(int id, int laudoPaiId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE LAUDO SET Laudo_complementar_id = LaudoComplementarId WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", laudoPaiId);
            cmd.Parameters.AddWithValue("LaudoComplementarId", laudoPaiId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public byte[] GerarPdf(Laudo laudo)
        {
            LaudoPdfService lp = new LaudoPdfService();
            return lp.GerarPdf(laudo);
        }
    }
}
