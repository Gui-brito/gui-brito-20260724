using GestaoColaboradores.API.Data;
using GestaoColaboradores.API.Models;
using GestaoColaboradores.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestaoColaboradores.API.Repositories;

public class ColaboradorRepository : IColaboradorRepository
{
    private readonly AppDbContext _context;

    public ColaboradorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Colaborador>> GetAllAsync()
    {
        return await _context.Colaboradores
            .AsNoTracking()
            .Include(c => c.Unidade)
            .Include(c => c.Usuario)
            .ToListAsync();
    }

    public async Task<Colaborador?> GetByIdAsync(int id)
    {
        return await _context.Colaboradores
            .Include(c => c.Unidade)
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Colaborador> CreateAsync(Colaborador colaborador)
    {
        _context.Colaboradores.Add(colaborador);
        await _context.SaveChangesAsync();
        return colaborador;
    }

    public async Task UpdateAsync(Colaborador colaborador)
    {
        _context.Colaboradores.Update(colaborador);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Colaborador colaborador)
    {
        _context.Colaboradores.Remove(colaborador);
        await _context.SaveChangesAsync();
    }
}
