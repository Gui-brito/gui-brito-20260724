using GestaoColaboradores.API.Models;

namespace GestaoColaboradores.API.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<IEnumerable<Usuario>> GetByStatusAsync(bool ativo);
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario?> GetByLoginAsync(string login);
    Task<Usuario> CreateAsync(Usuario usuario);
    Task UpdateAsync(Usuario usuario);
}
