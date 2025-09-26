using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace MicroEmpresa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosOnlineController : ControllerBase
    {
        private readonly IUsuariosOnlineLogic _logic;
        public UsuariosOnlineController(IUsuariosOnlineLogic logic) => _logic = logic;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuariosOnlineEntity>>> ListarAsync()
            => Ok(await _logic.ListarAsync());

        [HttpGet("by-loja/{idLoja:int}")]
        public async Task<ActionResult<IEnumerable<UsuariosOnlineEntity>>> ListarPorLojaAsync(int idLoja)
            => Ok(await _logic.ListarPorLojaAsync(idLoja));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuariosOnlineEntity>> ObterAsync(int id)
        {
            var e = await _logic.ObterAsync(id);
            return e is null ? NotFound() : Ok(e);
        }

        [HttpGet("by-loja-login")]
        public async Task<ActionResult<UsuariosOnlineEntity>> ObterPorLojaLoginAsync([FromQuery] int idLoja, [FromQuery] string login)
        {
            var e = await _logic.ObterPorLojaLoginAsync(idLoja, login);
            return e is null ? NotFound() : Ok(e);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMessage>> CriarAsync([FromBody] UsuariosOnlineEntity entity)
        {
            var r = await _logic.CriarAsync(entity);
            if (!string.Equals(r.Message, "OK", StringComparison.OrdinalIgnoreCase))
                return BadRequest(r);

            var criado = await _logic.ObterPorLojaLoginAsync(entity.IdLoja, entity.Login);
            return CreatedAtAction(nameof(ObterAsync), new { id = criado?.Id }, r);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ResponseMessage>> AtualizarAsync(int id, [FromBody] UsuariosOnlineEntity entity)
        {
            entity.Id = id;
            var r = await _logic.AtualizarAsync(entity);

            if (r.Message == "OK") return Ok(r);
            if (r.Message.Contains("não encontrado", StringComparison.OrdinalIgnoreCase)) return NotFound(r);
            if (r.Message.StartsWith("Concorrência", StringComparison.OrdinalIgnoreCase)) return Conflict(r);
            return BadRequest(r);
        }

        [HttpPatch("{id:int}/senha")]
        public async Task<ActionResult<ResponseMessage>> AtualizarSenhaAsync(int id, [FromBody] AlterarSenhaDto body)
        {
            var r = await _logic.AtualizarSenhaAsync(id, body.NovaSenhaHash, body.Rv);
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

    public class AlterarSenhaDto
    {
        public byte[] NovaSenhaHash { get; set; } = Array.Empty<byte>();
        public byte[] Rv { get; set; } = Array.Empty<byte>();
    }
}
