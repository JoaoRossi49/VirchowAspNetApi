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
                    DatNascimento DATE NOT NULL,
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
                    ExameId INTEGER,
                    Laudo_complementar_id INTEGER
                );";
            tableCmd.ExecuteNonQuery();
        }

        public List<Laudo> GetAll(bool invalidados)
        {
            var laudos = new List<Laudo>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            if (invalidados)
                cmd.CommandText = "SELECT * FROM Laudo WHERE DatInvalidado is not null";
            if (!invalidados)
                cmd.CommandText = "SELECT * FROM Laudo WHERE DatInvalidado is null";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ExameService exameService = new ExameService();
                laudos.Add(new Laudo
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    NroLaudo = reader.IsDBNull(reader.GetOrdinal("NroLaudo")) ? null : reader.GetInt32(reader.GetOrdinal("NroLaudo")),
                    NomePaciente = reader.GetString(reader.GetOrdinal("NomePaciente")),
                    Idade = reader.IsDBNull(reader.GetOrdinal("Idade")) ? null : reader.GetString(reader.GetOrdinal("Idade")),
                    DatNascimento = reader.IsDBNull(reader.GetOrdinal("DatNascimento")) ? null : reader.GetDateTime(reader.GetOrdinal("DatNascimento")),
                    EstadoCivil = reader.IsDBNull(reader.GetOrdinal("EstadoCivil")) ? null : reader.GetString(reader.GetOrdinal("EstadoCivil")),
                    ResumoClinico = reader.IsDBNull(reader.GetOrdinal("ResumoClinico")) ? null : reader.GetString(reader.GetOrdinal("ResumoClinico")),
                    HipoteseDiagnostica = reader.IsDBNull(reader.GetOrdinal("HipoteseDiagnostica")) ? null : reader.GetString(reader.GetOrdinal("HipoteseDiagnostica")),
                    DatUltimaMenstruacao = reader.IsDBNull(reader.GetOrdinal("DatUltimaMenstruacao")) ? null : reader.GetDateTime(reader.GetOrdinal("DatUltimaMenstruacao")),
                    MedicoRequisitante = reader.IsDBNull(reader.GetOrdinal("MedicoRequisitante")) ? null : reader.GetString(reader.GetOrdinal("MedicoRequisitante")),
                    DatExame = reader.IsDBNull(reader.GetOrdinal("DatExame")) ? null : reader.GetDateTime(reader.GetOrdinal("DatExame")),
                    DatInclusao = reader.IsDBNull(reader.GetOrdinal("DatInclusao")) ? null : reader.GetDateTime(reader.GetOrdinal("DatInclusao")),
                    DatImpressao = reader.IsDBNull(reader.GetOrdinal("DatImpressao")) ? null : reader.GetDateTime(reader.GetOrdinal("DatImpressao")),
                    DesLaudo = reader.IsDBNull(reader.GetOrdinal("DesLaudo")) ? null : reader.GetString(reader.GetOrdinal("DesLaudo")),
                    DatInvalidado = reader.IsDBNull(reader.GetOrdinal("DatInvalidado")) ? null : reader.GetDateTime(reader.GetOrdinal("DatInvalidado")),
                    UsuarioInvalidaId = reader.IsDBNull(reader.GetOrdinal("Usuario_Invalida_Id")) ? null : reader.GetInt32(reader.GetOrdinal("Usuario_Invalida_Id")),
                    Exame = exameService.GetById(reader.IsDBNull(reader.GetOrdinal("ExameId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ExameId")))
                });
            }

            return laudos;
        }

        public List<Laudo> GetByFilter(LaudoFilter laudo)
        {
            var laudos = new List<Laudo>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            string query = "SELECT * FROM Laudo p";

            if (laudo.NroLaudo > 0)
            {
                query += $" AND p.NroLaudo LIKE '%{laudo.NroLaudo}%'";
            }

            if (!string.IsNullOrEmpty(laudo.NomePaciente))
            {
                query += $" AND p.NomePaciente LIKE '%{laudo.NomePaciente}%'";
            }

            if (laudo.DatNascimento != null)
            {
                query += $" AND p.Dat_nascimento = '{laudo.DatNascimento:yyyy-MM-ddTHH:mm:ss.fff}'";
            }

            if (laudo.DatInclusaoInicial != null && laudo.DatInclusaoFinal != null)
            {
                query += $" AND p.DatInclusao BETWEEN '{laudo.DatInclusaoInicial:yyyy-MM-ddTHH:mm:ss.fff}' and '{laudo.DatInclusaoFinal:yyyy-MM-ddTHH:mm:ss.fff}'";
            }

            cmd.CommandText = query;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ExameService exameService = new ExameService();
                laudos.Add(new Laudo
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    NroLaudo = reader.IsDBNull(reader.GetOrdinal("NroLaudo")) ? null : reader.GetInt32(reader.GetOrdinal("NroLaudo")),
                    NomePaciente = reader.GetString(reader.GetOrdinal("NomePaciente")),
                    Idade = reader.IsDBNull(reader.GetOrdinal("Idade")) ? null : reader.GetString(reader.GetOrdinal("Idade")),
                    DatNascimento = reader.IsDBNull(reader.GetOrdinal("DatNascimento")) ? null : reader.GetDateTime(reader.GetOrdinal("DatNascimento")),
                    EstadoCivil = reader.IsDBNull(reader.GetOrdinal("EstadoCivil")) ? null : reader.GetString(reader.GetOrdinal("EstadoCivil")),
                    ResumoClinico = reader.IsDBNull(reader.GetOrdinal("ResumoClinico")) ? null : reader.GetString(reader.GetOrdinal("ResumoClinico")),
                    HipoteseDiagnostica = reader.IsDBNull(reader.GetOrdinal("HipoteseDiagnostica")) ? null : reader.GetString(reader.GetOrdinal("HipoteseDiagnostica")),
                    DatUltimaMenstruacao = reader.IsDBNull(reader.GetOrdinal("DatUltimaMenstruacao")) ? null : reader.GetDateTime(reader.GetOrdinal("DatUltimaMenstruacao")),
                    MedicoRequisitante = reader.IsDBNull(reader.GetOrdinal("MedicoRequisitante")) ? null : reader.GetString(reader.GetOrdinal("MedicoRequisitante")),
                    DatExame = reader.IsDBNull(reader.GetOrdinal("DatExame")) ? null : reader.GetDateTime(reader.GetOrdinal("DatExame")),
                    DatInclusao = reader.IsDBNull(reader.GetOrdinal("DatInclusao")) ? null : reader.GetDateTime(reader.GetOrdinal("DatInclusao")),
                    DatImpressao = reader.IsDBNull(reader.GetOrdinal("DatImpressao")) ? null : reader.GetDateTime(reader.GetOrdinal("DatImpressao")),
                    DesLaudo = reader.IsDBNull(reader.GetOrdinal("DesLaudo")) ? null : reader.GetString(reader.GetOrdinal("DesLaudo")),
                    DatInvalidado = reader.IsDBNull(reader.GetOrdinal("DatInvalidado")) ? null : reader.GetDateTime(reader.GetOrdinal("DatInvalidado")),
                    UsuarioInvalidaId = reader.IsDBNull(reader.GetOrdinal("Usuario_Invalida_Id")) ? null : reader.GetInt32(reader.GetOrdinal("Usuario_Invalida_Id")),
                    Exame = exameService.GetById(reader.IsDBNull(reader.GetOrdinal("ExameId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ExameId")))
                });
            }

            return laudos;
        }

        public Laudo? GetById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Laudo WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);

            ExameService exameService = new ExameService();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Laudo
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    NroLaudo = reader.IsDBNull(reader.GetOrdinal("NroLaudo")) ? null : reader.GetInt32(reader.GetOrdinal("NroLaudo")),
                    NomePaciente = reader.GetString(reader.GetOrdinal("NomePaciente")),
                    Idade = reader.IsDBNull(reader.GetOrdinal("Idade")) ? null : reader.GetString(reader.GetOrdinal("Idade")),
                    DatNascimento = reader.IsDBNull(reader.GetOrdinal("DatNascimento")) ? null : reader.GetDateTime(reader.GetOrdinal("DatNascimento")),
                    EstadoCivil = reader.IsDBNull(reader.GetOrdinal("EstadoCivil")) ? null : reader.GetString(reader.GetOrdinal("EstadoCivil")),
                    ResumoClinico = reader.IsDBNull(reader.GetOrdinal("ResumoClinico")) ? null : reader.GetString(reader.GetOrdinal("ResumoClinico")),
                    HipoteseDiagnostica = reader.IsDBNull(reader.GetOrdinal("HipoteseDiagnostica")) ? null : reader.GetString(reader.GetOrdinal("HipoteseDiagnostica")),
                    DatUltimaMenstruacao = reader.IsDBNull(reader.GetOrdinal("DatUltimaMenstruacao")) ? null : reader.GetDateTime(reader.GetOrdinal("DatUltimaMenstruacao")),
                    MedicoRequisitante = reader.IsDBNull(reader.GetOrdinal("MedicoRequisitante")) ? null : reader.GetString(reader.GetOrdinal("MedicoRequisitante")),
                    DatExame = reader.IsDBNull(reader.GetOrdinal("DatExame")) ? null : reader.GetDateTime(reader.GetOrdinal("DatExame")),
                    DatInclusao = reader.IsDBNull(reader.GetOrdinal("DatInclusao")) ? null : reader.GetDateTime(reader.GetOrdinal("DatInclusao")),
                    DatImpressao = reader.IsDBNull(reader.GetOrdinal("DatImpressao")) ? null : reader.GetDateTime(reader.GetOrdinal("DatImpressao")),
                    DesLaudo = reader.IsDBNull(reader.GetOrdinal("DesLaudo")) ? null : reader.GetString(reader.GetOrdinal("DesLaudo")),
                    DatInvalidado = reader.IsDBNull(reader.GetOrdinal("DatInvalidado")) ? null : reader.GetDateTime(reader.GetOrdinal("DatInvalidado")),
                    UsuarioInvalidaId = reader.IsDBNull(reader.GetOrdinal("Usuario_Invalida_Id")) ? null : reader.GetInt32(reader.GetOrdinal("Usuario_Invalida_Id")),
                    Exame = exameService.GetById(reader.IsDBNull(reader.GetOrdinal("ExameId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ExameId")))

                };
            }

            return null;
        }

        public LaudoRequest Add(LaudoRequest laudo)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT
	                                INTO
	                                Laudo 
                                   (NomePaciente,
	                                Idade,
	                                EstadoCivil,
	                                ResumoClinico,
	                                HipoteseDiagnostica,
	                                DatUltimaMenstruacao,
	                                MedicoRequisitante,
                                    DatNascimento,
	                                DatExame,
	                                DatInclusao,
	                                DatImpressao,
	                                DesLaudo,
	                                ExameId)
                                VALUES 
                               ($nomePaciente,
                                $idade,
                                $estadoCivil,
                                $resumoClinico,
                                $hipotese,
                                $datUltMenstr,
                                $medico,
                                $datNascimento,
                                $datExame,
                                DATETIME('now',
                                'localtime'),
                                DATETIME('now',
                                'localtime'),
                                $desLaudo,
                                $exameId);

                                SELECT
	                                last_insert_rowid();";

            cmd.Parameters.AddWithValue("$nomePaciente", laudo.NomePaciente);
            cmd.Parameters.AddWithValue("$idade", laudo.Idade ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$estadoCivil", laudo.EstadoCivil ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$resumoClinico", laudo.ResumoClinico ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$hipotese", laudo.HipoteseDiagnostica ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datUltMenstr", laudo.DatUltimaMenstruacao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$medico", laudo.MedicoRequisitante ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datNascimento", laudo.DatNascimento ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datExame", laudo.DatExame ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$desLaudo", laudo.DesLaudo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$exameId", laudo.ExameId > 0 ? laudo.ExameId : (object)DBNull.Value);


            laudo.Id = (int)(long)cmd.ExecuteScalar()!;
            return laudo;
        }

        public LaudoComplementarRequest Add(LaudoComplementarRequest laudo)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT
	                                INTO
	                                Laudo 
                                   (NomePaciente,
	                                Idade,
	                                EstadoCivil,
	                                ResumoClinico,
	                                HipoteseDiagnostica,
	                                DatUltimaMenstruacao,
	                                MedicoRequisitante,
                                    DatNascimento,
	                                DatExame,
	                                DatInclusao,
	                                DatImpressao,
	                                DesLaudo,
	                                Laudo_complementar_id)
                                VALUES 
                               ($nomePaciente,
                                $idade,
                                $estadoCivil,
                                $resumoClinico,
                                $hipotese,
                                $datUltMenstr,
                                $medico,
                                $datNascimento,
                                $datExame,
                                DATETIME('now',
                                'localtime'),
                                DATETIME('now',
                                'localtime'),
                                $desLaudo,
                                $exameId,
                                $laudoComplementarId);

                                SELECT
	                                last_insert_rowid();";

            cmd.Parameters.AddWithValue("$nomePaciente", laudo.NomePaciente);
            cmd.Parameters.AddWithValue("$idade", laudo.Idade ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$estadoCivil", laudo.EstadoCivil ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$resumoClinico", laudo.ResumoClinico ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$hipotese", laudo.HipoteseDiagnostica ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datUltMenstr", laudo.DatUltimaMenstruacao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$medico", laudo.MedicoRequisitante ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datNascimento", laudo.DatNascimento ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$datExame", laudo.DatExame ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$desLaudo", laudo.DesLaudo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$exameId", laudo.ExameId > 0 ? laudo.ExameId : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$laudoComplementarId", laudo.LaudoComplementarId);


            laudo.Id = (int)(long)cmd.ExecuteScalar()!;
            return laudo;
        }

        public bool Update(int id, LaudoRequest laudo)
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
                    DatImpressao = DATETIME('now', 'localtime'),
                    DesLaudo = $desLaudo,
                    ExameId = $exameId
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
            cmd.Parameters.AddWithValue("$desLaudo", laudo.DesLaudo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$exameId", laudo.ExameId > 0 ? laudo.ExameId : (object)DBNull.Value);

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
