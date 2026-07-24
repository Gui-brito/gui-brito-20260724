using GestaoColaboradores.API.Models.DTOs;

namespace GestaoColaboradores.API.Services.Interfaces;

public interface IColaboradorService
{
    Task<IEnumerable<ColaboradorResponseDto>> GetAllAsync();
    Task<ColaboradorResponseDto> CreateAsync(CriarColaboradorDto dto);
    Task<ColaboradorResponseDto> UpdateAsync(int id, AtualizarColaboradorDto dto);
    Task DeleteAsync(int id);
}
