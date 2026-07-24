namespace GestaoColaboradores.API.Models.DTOs;

public class CriarUnidadeDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
}

public class AtualizarUnidadeDto
{
    public bool Ativa { get; set; }
}

public class UnidadeResponseDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public bool Ativa { get; set; }
    public List<ColaboradorResumoDto> Colaboradores { get; set; } = new();
}

public class ColaboradorResumoDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
}
