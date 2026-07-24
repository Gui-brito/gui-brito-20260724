using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestaoColaboradores.API.Models.DTOs;
using GestaoColaboradores.API.Repositories.Interfaces;
using GestaoColaboradores.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace GestaoColaboradores.API.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        var usuario = await _usuarioRepository.GetByLoginAsync(dto.Login)
            ?? throw new UnauthorizedAccessException("Login ou senha inválidos.");

        if (!usuario.Ativo)
            throw new UnauthorizedAccessException("Usuário inativo.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            throw new UnauthorizedAccessException("Login ou senha inválidos.");

        var token = GenerateToken(usuario.Id, usuario.Login);

        return new LoginResponseDto
        {
            Token = token,
            Usuario = new UsuarioResponseDto
            {
                Id = usuario.Id,
                Codigo = usuario.Codigo,
                Login = usuario.Login,
                Ativo = usuario.Ativo
            }
        };
    }

    private string GenerateToken(int userId, string login)
    {
        var key = _configuration["Jwt:Key"] ?? "ChaveSecretaParaDesenvolvimento2024!@#$";
        var expirationHours = int.Parse(_configuration["Jwt:ExpirationInHours"] ?? "8");

        var tokenHandler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.ASCII.GetBytes(key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, login)
            }),
            Expires = DateTime.UtcNow.AddHours(expirationHours),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
