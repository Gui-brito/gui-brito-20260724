using GestaoColaboradores.API.Models.DTOs;
using GestaoColaboradores.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoColaboradores.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuariosController(IUsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetAll([FromQuery] bool? ativo)
    {
        if (ativo.HasValue)
        {
            var filtrados = await _service.GetByStatusAsync(ativo.Value);
            return Ok(filtrados);
        }

        var usuarios = await _service.GetAllAsync();
        return Ok(usuarios);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioResponseDto>> Create([FromBody] CriarUsuarioDto dto)
    {
        try
        {
            var usuario = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { }, usuario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UsuarioResponseDto>> Update(int id, [FromBody] AtualizarUsuarioDto dto)
    {
        try
        {
            var usuario = await _service.UpdateAsync(id, dto);
            return Ok(usuario);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
