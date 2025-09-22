using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace MicroEmpresa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentosController : ControllerBase
    {
        private readonly IPagamentosLogic _logic;
        public PagamentosController(IPagamentosLogic logic) => _logic = logic;

        [HttpGet("por-venda/{idVenda:int}")]
        public async Task<ActionResult<IEnumerable<PagamentosEntity>>> ListarPorVenda(int idVenda)
            => Ok(await _logic.ListarPorVendaAsync(idVenda));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PagamentosEntity>> ObterAsync(int id)
        {
            var p = await _logic.ObterAsync(id);
            return p is null ? NotFound() : Ok(p);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMessage>> CriarAsync([FromBody] PagamentosEntity entity)
        {
            var r = await _logic.CriarAsync(entity);
            if (r.Message != "OK") return BadRequest(r);
            return CreatedAtAction(nameof(ObterAsync), new { id = int.Parse(r.Data) }, r);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ResponseMessage>> AtualizarAsync(int id, [FromBody] PagamentosEntity entity)
        {
            entity.Id = id;
            var r = await _logic.AtualizarAsync(entity);
            return r.Message == "OK" ? Ok(r) : Conflict(r);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseMessage>> ExcluirAsync(int id)
        {
            var r = await _logic.ExcluirAsync(id);
            return r.Message == "OK" ? Ok(r) : BadRequest(r);
        }
    }
}
