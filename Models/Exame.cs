namespace VirchowAspNetApi.Models
{
    public class Exame
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<string> TopicosList { get; set; }
    }
}
