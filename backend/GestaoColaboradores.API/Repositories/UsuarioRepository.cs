using GestaoColaboradores.API.Data;
using GestaoColaboradores.API.Models;
using GestaoColaboradores.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestaoColaboradores.API.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Usuario>> GetByStatusAsync(bool ativo)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .Where(u => u.Ativo == ativo)
            .ToListAsync();
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<Usuario?> GetByLoginAsync(string login)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }
}
