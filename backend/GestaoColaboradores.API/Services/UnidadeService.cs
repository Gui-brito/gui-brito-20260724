using GestaoColaboradores.API.Models;
using GestaoColaboradores.API.Models.DTOs;
using GestaoColaboradores.API.Repositories.Interfaces;
using GestaoColaboradores.API.Services.Interfaces;

namespace GestaoColaboradores.API.Services;

public class UnidadeService : IUnidadeService
{
    private readonly IUnidadeRepository _repository;

    public UnidadeService(IUnidadeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UnidadeResponseDto>> GetAllAsync()
    {
        var unidades = await _repository.GetAllWithColaboradoresAsync();
        return unidades.Select(MapToDto);
    }

    public async Task<UnidadeResponseDto> CreateAsync(CriarUnidadeDto dto)
    {
        var existente = await _repository.GetByCodigoAsync(dto.Codigo);
        if (existente != null)
            throw new InvalidOperationException("Já existe uma unidade com este código.");

        var unidade = new Unidade
        {
            Codigo = dto.Codigo,
            Nome = dto.Nome,
            Ativa = true
        };

        await _repository.CreateAsync(unidade);
        return MapToDto(unidade);
    }

    public async Task<UnidadeResponseDto> UpdateAsync(int id, AtualizarUnidadeDto dto)
    {
        var unidade = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Unidade não encontrada.");

        unidade.Ativa = dto.Ativa;

        await _repository.UpdateAsync(unidade);
        return MapToDto(unidade);
    }

    private static UnidadeResponseDto MapToDto(Unidade u) => new()
    {
        Id = u.Id,
        Codigo = u.Codigo,
        Nome = u.Nome,
        Ativa = u.Ativa,
        Colaboradores = u.Colaboradores?.Select(c => new ColaboradorResumoDto
        {
            Id = c.Id,
            Codigo = c.Codigo,
            Nome = c.Nome
        }).ToList() ?? new()
    };
}
