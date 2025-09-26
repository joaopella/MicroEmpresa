using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace MicroEmpresa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosLogic _logic;
        public ProdutosController(IProdutosLogic logic) => _logic = logic;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutosEntity>>> ListarAsync()
            => Ok(await _logic.ListarAsync());

        [HttpGet("by-loja/{idLoja:int}")]
        public async Task<ActionResult<IEnumerable<ProdutosEntity>>> ListarPorLojaAsync(int idLoja)
            => Ok(await _logic.ListarPorLojaAsync(idLoja));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProdutosEntity>> ObterAsync(int id)
        {
            var e = await _logic.ObterAsync(id);
            return e is null ? NotFound() : Ok(e);
        }

        [HttpGet("by-loja-sku")]
        public async Task<ActionResult<ProdutosEntity>> ObterPorLojaSkuAsync([FromQuery] int idLoja, [FromQuery] string sku)
        {
            var e = await _logic.ObterPorLojaSkuAsync(idLoja, sku);
            return e is null ? NotFound() : Ok(e);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMessage>> CriarAsync([FromBody] ProdutosEntity entity)
        {
            var r = await _logic.CriarAsync(entity);
            if (!string.Equals(r.Message, "OK", StringComparison.OrdinalIgnoreCase))
                return BadRequest(r);

            var criado = await _logic.ObterPorLojaSkuAsync(entity.IdLoja, entity.Sku);
            return CreatedAtAction(nameof(ObterAsync), new { id = criado?.Id }, r);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ResponseMessage>> AtualizarAsync(int id, [FromBody] ProdutosEntity entity)
        {
            entity.Id = id;
            var r = await _logic.AtualizarAsync(entity);

            if (r.Message == "OK") return Ok(r);
            if (r.Message.Contains("não encontrado", StringComparison.OrdinalIgnoreCase)) return NotFound(r);
            if (r.Message.StartsWith("Concorrência", StringComparison.OrdinalIgnoreCase)) return Conflict(r);

            return BadRequest(r);
        }

        // Atualização parcial de preços/markup com Rv
        [HttpPatch("{id:int}/precos")]
        public async Task<ActionResult<ResponseMessage>> AtualizarPrecosAsync(
            int id,
            [FromQuery] decimal? precoVenda,
            [FromQuery] decimal? custo,
            [FromQuery(Name = "markup")] decimal? markupPercentual,
            [FromBody] byte[] rv)
        {
            var r = await _logic.AtualizarPrecosAsync(id, precoVenda, custo, markupPercentual, rv);
            if (r.Message == "OK") return Ok(r);
            if (r.Message.Contains("não encontrado", StringComparison.OrdinalIgnoreCase)) return NotFound(r);
            if (r.Message.StartsWith("Concorrência", StringComparison.OrdinalIgnoreCase)) return Conflict(r);

            return BadRequest(r);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseMessage>> ExcluirAsync(int id)
        {
            var r = await _logic.ExcluirAsync(id);
            return r.Message == "OK" ? Ok(r) : NotFound(r);
        }
    }
}
