namespace GestaoColaboradores.API.Models;

public class Unidade : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public bool Ativa { get; set; } = true;

    public ICollection<Colaborador> Colaboradores { get; set; } = new List<Colaborador>();
}
