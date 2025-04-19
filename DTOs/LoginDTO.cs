using VirchowAspNetApi.Models;

namespace VirchowAspNetApi.DTOs
{
    public class LoginRequest
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public Usuario Usuario { get; set; } = null!;
    }

}
