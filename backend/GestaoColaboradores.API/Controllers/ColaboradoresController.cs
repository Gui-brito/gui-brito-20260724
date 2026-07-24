using GestaoColaboradores.API.Models.DTOs;
using GestaoColaboradores.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoColaboradores.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ColaboradoresController : ControllerBase
{
    private readonly IColaboradorService _service;

    public ColaboradoresController(IColaboradorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ColaboradorResponseDto>>> GetAll()
    {
        var colaboradores = await _service.GetAllAsync();
        return Ok(colaboradores);
    }

    [HttpPost]
    public async Task<ActionResult<ColaboradorResponseDto>> Create([FromBody] CriarColaboradorDto dto)
    {
        try
        {
            var colaborador = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { }, colaborador);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ColaboradorResponseDto>> Update(int id, [FromBody] AtualizarColaboradorDto dto)
    {
        try
        {
            var colaborador = await _service.UpdateAsync(id, dto);
            return Ok(colaborador);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
