using GestaoColaboradores.API.Models.DTOs;

namespace GestaoColaboradores.API.Services.Interfaces;

public interface IUnidadeService
{
    Task<IEnumerable<UnidadeResponseDto>> GetAllAsync();
    Task<UnidadeResponseDto> CreateAsync(CriarUnidadeDto dto);
    Task<UnidadeResponseDto> UpdateAsync(int id, AtualizarUnidadeDto dto);
}
