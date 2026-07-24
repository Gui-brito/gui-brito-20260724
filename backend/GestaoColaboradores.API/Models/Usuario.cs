namespace GestaoColaboradores.API.Models;

public class Usuario : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;

    public Colaborador? Colaborador { get; set; }
}
