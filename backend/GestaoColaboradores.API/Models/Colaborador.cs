namespace GestaoColaboradores.API.Models;

public class Colaborador : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;

    public int UnidadeId { get; set; }
    public Unidade Unidade { get; set; } = null!;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
}
