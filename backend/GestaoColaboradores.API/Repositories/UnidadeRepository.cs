using GestaoColaboradores.API.Data;
using GestaoColaboradores.API.Models;
using GestaoColaboradores.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestaoColaboradores.API.Repositories;

public class UnidadeRepository : IUnidadeRepository
{
    private readonly AppDbContext _context;

    public UnidadeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Unidade>> GetAllWithColaboradoresAsync()
    {
        return await _context.Unidades
            .AsNoTracking()
            .Include(u => u.Colaboradores)
            .ToListAsync();
    }

    public async Task<Unidade?> GetByIdAsync(int id)
    {
        return await _context.Unidades
            .Include(u => u.Colaboradores)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Unidade?> GetByCodigoAsync(string codigo)
    {
        return await _context.Unidades
            .FirstOrDefaultAsync(u => u.Codigo == codigo);
    }

    public async Task<Unidade> CreateAsync(Unidade unidade)
    {
        _context.Unidades.Add(unidade);
        await _context.SaveChangesAsync();
        return unidade;
    }

    public async Task UpdateAsync(Unidade unidade)
    {
        _context.Unidades.Update(unidade);
        await _context.SaveChangesAsync();
    }
}
