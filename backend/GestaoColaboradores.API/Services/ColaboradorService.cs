using GestaoColaboradores.API.Models;
using GestaoColaboradores.API.Models.DTOs;
using GestaoColaboradores.API.Repositories.Interfaces;
using GestaoColaboradores.API.Services.Interfaces;

namespace GestaoColaboradores.API.Services;

public class ColaboradorService : IColaboradorService
{
    private readonly IColaboradorRepository _repository;
    private readonly IUnidadeRepository _unidadeRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public ColaboradorService(
        IColaboradorRepository repository,
        IUnidadeRepository unidadeRepository,
        IUsuarioRepository usuarioRepository)
    {
        _repository = repository;
        _unidadeRepository = unidadeRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<IEnumerable<ColaboradorResponseDto>> GetAllAsync()
    {
        var colaboradores = await _repository.GetAllAsync();
        return colaboradores.Select(MapToDto);
    }

    public async Task<ColaboradorResponseDto> CreateAsync(CriarColaboradorDto dto)
    {
        var unidade = await _unidadeRepository.GetByIdAsync(dto.UnidadeId)
            ?? throw new KeyNotFoundException("Unidade não encontrada.");

        if (!unidade.Ativa)
            throw new InvalidOperationException("Não é possível associar colaborador a uma unidade inativa.");

        var usuario = await _usuarioRepository.GetByIdAsync(dto.UsuarioId)
            ?? throw new KeyNotFoundException("Usuário não encontrado.");

        var colaborador = new Colaborador
        {
            Codigo = Guid.NewGuid().ToString("N")[..8].ToUpper(),
            Nome = dto.Nome,
            UnidadeId = dto.UnidadeId,
            UsuarioId = dto.UsuarioId
        };

        await _repository.CreateAsync(colaborador);

        // Reload with includes
        var created = await _repository.GetByIdAsync(colaborador.Id);
        return MapToDto(created!);
    }

    public async Task<ColaboradorResponseDto> UpdateAsync(int id, AtualizarColaboradorDto dto)
    {
        var colaborador = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Colaborador não encontrado.");

        if (!string.IsNullOrWhiteSpace(dto.Nome))
            colaborador.Nome = dto.Nome;

        if (dto.UnidadeId.HasValue)
        {
            var unidade = await _unidadeRepository.GetByIdAsync(dto.UnidadeId.Value)
                ?? throw new KeyNotFoundException("Unidade não encontrada.");

            if (!unidade.Ativa)
                throw new InvalidOperationException("Não é possível associar colaborador a uma unidade inativa.");

            colaborador.UnidadeId = dto.UnidadeId.Value;
        }

        await _repository.UpdateAsync(colaborador);

        var updated = await _repository.GetByIdAsync(colaborador.Id);
        return MapToDto(updated!);
    }

    public async Task DeleteAsync(int id)
    {
        var colaborador = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Colaborador não encontrado.");

        await _repository.DeleteAsync(colaborador);
    }

    private static ColaboradorResponseDto MapToDto(Colaborador c) => new()
    {
        Id = c.Id,
        Codigo = c.Codigo,
        Nome = c.Nome,
        UnidadeNome = c.Unidade?.Nome ?? string.Empty,
        UnidadeId = c.UnidadeId,
        UsuarioId = c.UsuarioId,
        UsuarioLogin = c.Usuario?.Login ?? string.Empty
    };
}
