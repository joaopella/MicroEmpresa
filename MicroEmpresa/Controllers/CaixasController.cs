using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace MicroEmpresa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaixasController : ControllerBase
    {
        private readonly ICaixasLogic _logic;
        public CaixasController(ICaixasLogic logic) => _logic = logic;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaixasEntity>>> ListarAsync()
            => Ok(await _logic.ListarAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CaixasEntity>> ObterAsync(int id)
        {
            var c = await _logic.ObterAsync(id);
            return c is null ? NotFound() : Ok(c);
        }

        [HttpGet("aberto/{idLoja:int}")]
        public async Task<ActionResult<CaixasEntity>> ObterAbertoPorLojaAsync(int idLoja)
        {
            var c = await _logic.ObterAbertoPorLojaAsync(idLoja);
            return c is null ? NotFound() : Ok(c);
        }

        // ABRIR
        [HttpPost("abrir")]
        public async Task<ActionResult<ResponseMessage>> AbrirAsync([FromBody] CaixasEntity entity)
        {
            var r = await _logic.AbrirAsync(entity);
            if (r.Message != "OK") return BadRequest(r);

            // r.Data = Id do novo caixa
            return CreatedAtAction(nameof(ObterAsync), new { id = int.Parse(r.Data) }, r);
        }

        // FECHAR
        public record FecharRequest(byte[] Rv, int IdFuncionarioFechamento, decimal ValorFechamento, DateTime? DataFechamento);
        [HttpPost("{id:int}/fechar")]
        public async Task<ActionResult<ResponseMessage>> FecharAsync(int id, [FromBody] FecharRequest req)
        {
            var r = await _logic.FecharAsync(id, req.Rv, req.IdFuncionarioFechamento, req.ValorFechamento, req.DataFechamento);
            return r.Message == "OK" ? Ok(r) : Conflict(r);
        }

        // Atualizar observação (concorrência)
        public record ObsRequest(byte[] Rv, string? Obs);
        [HttpPatch("{id:int}/obs")]
        public async Task<ActionResult<ResponseMessage>> AtualizarObsAsync(int id, [FromBody] ObsRequest req)
        {
            var r = await _logic.AtualizarObsAsync(id, req.Rv, req.Obs);
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
