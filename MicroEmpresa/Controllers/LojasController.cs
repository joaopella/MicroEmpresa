using MicroEmpresa.Entity;
using MicroEmpresa.Logic.Lojas;
using Microsoft.AspNetCore.Mvc;
using System;

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
        LojasEntity lojasEntity = new LojasEntity();
        try
        {
            lojasEntity = await _svc.ObterAsync(id);

            return lojasEntity is null ? NotFound() : Ok(lojasEntity);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        
    }

    [HttpPost]
    public async Task<ActionResult<LojasEntity>> CriarLoja([FromBody] LojasEntity entity)
    {
        try
        {
            await _svc.CriarAsync(entity);
            return Ok(new { message = "Loja cadastrada com sucesso!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<LojasEntity>> Atualizar(int id, [FromBody] LojasEntity entity)
    {
        try
        {
            await _svc.AtualizarAsync(id, entity);

            return Ok(new { message = "Loja atualizada com sucesso!" });
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
