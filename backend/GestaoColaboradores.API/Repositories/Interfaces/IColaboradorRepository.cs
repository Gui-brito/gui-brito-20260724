using GestaoColaboradores.API.Models;

namespace GestaoColaboradores.API.Repositories.Interfaces;

public interface IColaboradorRepository
{
    Task<IEnumerable<Colaborador>> GetAllAsync();
    Task<Colaborador?> GetByIdAsync(int id);
    Task<Colaborador> CreateAsync(Colaborador colaborador);
    Task UpdateAsync(Colaborador colaborador);
    Task DeleteAsync(Colaborador colaborador);
}
