using GestaoColaboradores.API.Models.DTOs;

namespace GestaoColaboradores.API.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto dto);
}
