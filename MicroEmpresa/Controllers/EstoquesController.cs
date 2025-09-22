using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace MicroEmpresa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstoquesController : ControllerBase
    {
        private readonly IEstoquesLogic _logic;

        public EstoquesController(IEstoquesLogic logic) => _logic = logic;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstoquesEntity>>> ListarAsync()
            => Ok(await _logic.ListarAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EstoquesEntity>> ObterAsync(int id)
        {
            var e = await _logic.ObterAsync(id);
            return e is null ? NotFound() : Ok(e);
        }

        // Consulta direta pela composição Loja+Produto
        [HttpGet("by-loja-produto")]
        public async Task<ActionResult<EstoquesEntity>> ObterPorLojaProdutoAsync([FromQuery] int idLoja, [FromQuery] int idProduto)
        {
            var e = await _logic.ObterPorLojaProdutoAsync(idLoja, idProduto);
            return e is null ? NotFound() : Ok(e);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMessage>> CriarAsync([FromBody] EstoquesEntity entity)
        {
            var r = await _logic.CriarAsync(entity);
            if (!string.Equals(r.Message, "OK", StringComparison.OrdinalIgnoreCase))
                return BadRequest(r);

            // retorna o recurso criado
            var criado = await _logic.ObterPorLojaProdutoAsync(entity.IdLoja, entity.IdProduto);
            return CreatedAtAction(nameof(ObterAsync), new { id = criado?.Id }, r);
        }

        // Atualiza saldo e/ou chaves (respeitando rowversion)
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ResponseMessage>> AtualizarAsync(int id, [FromBody] EstoquesEntity entity)
        {
            entity.Id = id;
            var r = await _logic.AtualizarAsync(entity);
            if (r.Message == "OK") return Ok(r);

            // mensagens específicas controladas pela lógica
            return Conflict(r);
        }

        // Ajuste pontual no saldo (delta positivo/negativo) com rowversion obrigatório
        [HttpPatch("{id:int}/ajustar-saldo")]
        public async Task<ActionResult<ResponseMessage>> AjustarSaldoAsync(int id, [FromQuery] decimal delta, [FromBody] byte[] rv)
        {
            var r = await _logic.AjustarSaldoAsync(id, delta, rv);
            return r.Message == "OK" ? Ok(r) : Conflict(r);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseMessage>> ExcluirAsync(int id)
        {
            var r = await _logic.ExcluirAsync(id);
            return r.Message == "OK" ? Ok(r) : NotFound(r);
        }
    }
}
