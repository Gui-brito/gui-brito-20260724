using GestaoColaboradores.API.Models;

namespace GestaoColaboradores.API.Repositories.Interfaces;

public interface IUnidadeRepository
{
    Task<IEnumerable<Unidade>> GetAllWithColaboradoresAsync();
    Task<Unidade?> GetByIdAsync(int id);
    Task<Unidade?> GetByCodigoAsync(string codigo);
    Task<Unidade> CreateAsync(Unidade unidade);
    Task UpdateAsync(Unidade unidade);
}
