using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace MicroEmpresa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovEstoqueController : ControllerBase
    {
        private readonly IMovEstoqueLogic _logic;
        public MovEstoqueController(IMovEstoqueLogic logic) => _logic = logic;

        [HttpGet("por-loja-produto")]
        public async Task<ActionResult<IEnumerable<MovEstoqueEntity>>> Listar([FromQuery] int idLoja, [FromQuery] int idProduto)
            => Ok(await _logic.ListarPorLojaProdutoAsync(idLoja, idProduto));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovEstoqueEntity>> ObterAsync(int id)
        {
            var m = await _logic.ObterAsync(id);
            return m is null ? NotFound() : Ok(m);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMessage>> CriarAsync([FromBody] MovEstoqueEntity entity)
        {
            var r = await _logic.CriarAsync(entity);
            if (r.Message != "OK") return BadRequest(r);

            return CreatedAtAction(nameof(ObterAsync), new { id = int.Parse(r.Data) }, r);
        }

        // estornar=true desfaz o impacto no saldo
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseMessage>> ExcluirAsync(int id, [FromQuery] bool estornar = false)
        {
            var r = await _logic.ExcluirAsync(id, estornar);
            return r.Message == "OK" ? Ok(r) : BadRequest(r);
        }
    }
}
