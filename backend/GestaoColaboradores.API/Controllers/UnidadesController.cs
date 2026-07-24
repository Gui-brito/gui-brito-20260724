using GestaoColaboradores.API.Models.DTOs;
using GestaoColaboradores.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoColaboradores.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UnidadesController : ControllerBase
{
    private readonly IUnidadeService _service;

    public UnidadesController(IUnidadeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UnidadeResponseDto>>> GetAll()
    {
        var unidades = await _service.GetAllAsync();
        return Ok(unidades);
    }

    [HttpPost]
    public async Task<ActionResult<UnidadeResponseDto>> Create([FromBody] CriarUnidadeDto dto)
    {
        try
        {
            var unidade = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { }, unidade);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UnidadeResponseDto>> Update(int id, [FromBody] AtualizarUnidadeDto dto)
    {
        try
        {
            var unidade = await _service.UpdateAsync(id, dto);
            return Ok(unidade);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
