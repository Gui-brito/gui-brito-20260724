using GestaoColaboradores.API.Models.DTOs;

namespace GestaoColaboradores.API.Services.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioResponseDto>> GetAllAsync();
    Task<IEnumerable<UsuarioResponseDto>> GetByStatusAsync(bool ativo);
    Task<UsuarioResponseDto> CreateAsync(CriarUsuarioDto dto);
    Task<UsuarioResponseDto> UpdateAsync(int id, AtualizarUsuarioDto dto);
}
