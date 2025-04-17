namespace VirchowAspNetApi.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public string Sexo { get; set; }

        public DateTime DatNascimento { get; set; }

        public EstadoCivil EstadoCivil { get; set; }
    }
}
