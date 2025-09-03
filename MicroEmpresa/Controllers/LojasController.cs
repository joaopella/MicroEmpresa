using Microsoft.AspNetCore.Mvc;
using MicroEmpresa.Entity;
using MicroEmpresa.Logic.Lojas;

namespace MicroEmpresa.Controllers;

[ApiController]
[Route("api/lojas")]
public class LojasController : ControllerBase
{
    private readonly ILojasService _svc;
    public LojasController(ILojasService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LojasEntity>>> Listar()
        => Ok(await _svc.ListarAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LojasEntity>> Obter(int id)
    {
        var loja = await _svc.ObterAsync(id);
        return loja is null ? NotFound() : Ok(loja);
    }

    [HttpPost]
    public async Task<ActionResult<LojasEntity>> Criar([FromBody] LojasEntity entity)
    {
        try
        {
            var created = await _svc.CriarAsync(entity);
            return CreatedAtAction(nameof(Obter), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<LojasEntity>> Atualizar(int id, [FromBody] LojasEntity entity)
    {
        try
        {
            var updated = await _svc.AtualizarAsync(id, entity);
            return Ok(updated);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            return await _svc.RemoverAsync(id) ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
