using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace MicroEmpresa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovCaixaController : ControllerBase
    {
        private readonly IMovCaixaLogic _logic;
        public MovCaixaController(IMovCaixaLogic logic) => _logic = logic;

        [HttpGet("por-caixa/{idCaixa:int}")]
        public async Task<ActionResult<IEnumerable<MovCaixaEntity>>> ListarPorCaixa(int idCaixa)
            => Ok(await _logic.ListarPorCaixaAsync(idCaixa));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovCaixaEntity>> ObterAsync(int id)
        {
            var m = await _logic.ObterAsync(id);
            return m is null ? NotFound() : Ok(m);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMessage>> CriarAsync([FromBody] MovCaixaEntity entity)
        {
            var r = await _logic.CriarAsync(entity);
            if (r.Message != "OK") return BadRequest(r);

            return CreatedAtAction(nameof(ObterAsync), new { id = int.Parse(r.Data) }, r);
        }

        public record ObsRequest(byte[] Rv, string? Descricao);
        [HttpPatch("{id:int}/descricao")]
        public async Task<ActionResult<ResponseMessage>> AtualizarDescricaoAsync(int id, [FromBody] ObsRequest req)
        {
            var r = await _logic.AtualizarDescricaoAsync(id, req.Rv, req.Descricao);
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
