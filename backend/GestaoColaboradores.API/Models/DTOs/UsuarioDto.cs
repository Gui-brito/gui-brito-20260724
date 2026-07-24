namespace GestaoColaboradores.API.Models.DTOs;

public class CriarUsuarioDto
{
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public class AtualizarUsuarioDto
{
    public string? Senha { get; set; }
    public bool? Ativo { get; set; }
}

public class UsuarioResponseDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}

public class LoginDto
{
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UsuarioResponseDto Usuario { get; set; } = null!;
}
