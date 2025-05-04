using System.Diagnostics.Contracts;

namespace VirchowAspNetApi.Models
{
    public class Laudo
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
        public DateTime? DatImpressao { get; set; }
        public DateTime? DatInvalidado { get; set; }
        public int? UsuarioInvalidaId { get; set; }
        public TipoLaudo? TipoLaudo { get; set; }
        public string DesLaudo { get; set; }
    }
}
