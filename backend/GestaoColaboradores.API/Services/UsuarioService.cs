using GestaoColaboradores.API.Models;
using GestaoColaboradores.API.Models.DTOs;
using GestaoColaboradores.API.Repositories.Interfaces;
using GestaoColaboradores.API.Services.Interfaces;

namespace GestaoColaboradores.API.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UsuarioResponseDto>> GetAllAsync()
    {
        var usuarios = await _repository.GetAllAsync();
        return usuarios.Select(MapToDto);
    }

    public async Task<IEnumerable<UsuarioResponseDto>> GetByStatusAsync(bool ativo)
    {
        var usuarios = await _repository.GetByStatusAsync(ativo);
        return usuarios.Select(MapToDto);
    }

    public async Task<UsuarioResponseDto> CreateAsync(CriarUsuarioDto dto)
    {
        var existente = await _repository.GetByLoginAsync(dto.Login);
        if (existente != null)
            throw new InvalidOperationException("Já existe um usuário com este login.");

        var usuario = new Usuario
        {
            Codigo = Guid.NewGuid().ToString("N")[..8].ToUpper(),
            Login = dto.Login,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Ativo = true
        };

        await _repository.CreateAsync(usuario);
        return MapToDto(usuario);
    }

    public async Task<UsuarioResponseDto> UpdateAsync(int id, AtualizarUsuarioDto dto)
    {
        var usuario = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Usuário não encontrado.");

        if (!string.IsNullOrWhiteSpace(dto.Senha))
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

        if (dto.Ativo.HasValue)
            usuario.Ativo = dto.Ativo.Value;

        await _repository.UpdateAsync(usuario);
        return MapToDto(usuario);
    }

    private static UsuarioResponseDto MapToDto(Usuario usuario) => new()
    {
        Id = usuario.Id,
        Codigo = usuario.Codigo,
        Login = usuario.Login,
        Ativo = usuario.Ativo
    };
}
