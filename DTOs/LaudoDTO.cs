using VirchowAspNetApi.Models;

namespace VirchowAspNetApi.DTOs
{
    public class LaudoRequest
    {
        public int Id { get; set; }
        public string NomePaciente { get; set; }
        public string? Idade { get; set; }
        public string? EstadoCivil { get; set; }
        public string ResumoClinico { get; set; }
        public string HipoteseDiagnostica { get; set; }
        public DateTime? DatUltimaMenstruacao { get; set; }
        public string? MedicoRequisitante { get; set; }
        public DateTime? DatExame { get; set; }
        public string DesLaudo { get; set; }
    }

    public class LaudoComplementarRequest
    {
        public int Id { get; set; }
        public string NomePaciente { get; set; }
        public string? Idade { get; set; }
        public string? EstadoCivil { get; set; }
        public string ResumoClinico { get; set; }
        public string HipoteseDiagnostica { get; set; }
        public DateTime? DatUltimaMenstruacao { get; set; }
        public string? MedicoRequisitante { get; set; }
        public DateTime? DatExame { get; set; }
        public string DesLaudo { get; set; }
        public int LaudoComplementarId { get; set; }
    }

}
