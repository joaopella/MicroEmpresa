using MicroEmpresa.Entity;
using MicroEmpresa.Logic.Lojas;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MicroEmpresa.Controllers;

[ApiController]
[Route("api/lojas")]
public class LojasController : ControllerBase
{
    private readonly ILojasLogic _svc;
    public LojasController(ILojasLogic svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LojasEntity>>> Listar()
        => Ok(await _svc.ListarAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LojasEntity>> Obter(int id)
    {
        LojasEntity lojasEntity = new LojasEntity();
        
        try
        {
            lojasEntity = await _svc.ObterLoja(id);

            return Ok(lojasEntity);
        }
        catch (Exception ex)
        {
            return BadRequest(new { 
               message = ex.Message,
               data = ex.Data,
            });
        }
        
    }

    [HttpPost]
    public async Task<ActionResult<ResponseMessage>> CriarLoja([FromBody] LojasEntity entity)
    {
        try
        {
            return await _svc.CriarAsync(entity);
            
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message,
                data = ex.Data,
                teste = ex.Source,
                teste1 = ex.InnerException,
                teste2 = ex.StackTrace,
                tste3 = ex.HelpLink,
                teste4 = ex.TargetSite,
                teste5 = ex.HResult
            });
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
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message,
                data = ex.Data,
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ResponseMessage>> Remover(int id)
    {
        try
        {
            return await _svc.RemoverAsync(id);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message,
                data = ex.Data,
            });
        }
    }
}
