namespace GestaoColaboradores.API.Models.DTOs;

public class CriarColaboradorDto
{
    public string Nome { get; set; } = string.Empty;
    public int UnidadeId { get; set; }
    public int UsuarioId { get; set; }
}

public class AtualizarColaboradorDto
{
    public string? Nome { get; set; }
    public int? UnidadeId { get; set; }
}

public class ColaboradorResponseDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string UnidadeNome { get; set; } = string.Empty;
    public int UnidadeId { get; set; }
    public int UsuarioId { get; set; }
    public string UsuarioLogin { get; set; } = string.Empty;
}
