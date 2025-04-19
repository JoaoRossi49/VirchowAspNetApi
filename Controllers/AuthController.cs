using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VirchowAspNetApi.DTOs;
using VirchowAspNetApi.Models;
using VirchowAspNetApi.Services;
using VirchowAspNetApi.Utils;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UsuarioService _usuarioService;
    private readonly IConfiguration _config;

    public AuthController(UsuarioService usuarioService, IConfiguration config)
    {
        _usuarioService = usuarioService;
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var usuario = _usuarioService.GetByLogin(request.Login);
        if (usuario == null || !PasswordHasher.VerifyPassword(request.Senha, usuario.Senha))
        {
            return Unauthorized(new { message = "Usuário ou senha inválidos" });
        }

        var token = GenerateJwtToken(usuario);

        var response = new LoginResponse
        {
            Token = token,
            Usuario = usuario
        };

        return Ok(response);
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim("Login", usuario.Login)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
