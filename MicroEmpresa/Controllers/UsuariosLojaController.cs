using global::MicroEmpresa.Entity;
using global::MicroEmpresa.LogicInterface;
using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MicroEmpresa.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosLojaController : ControllerBase
    {
        private readonly IUsuariosLojaLogic _logic;

        public UsuariosLojaController(IUsuariosLojaLogic logic) => _logic = logic;

        // GET: api/UsuariosLoja/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var e = await _logic.GetAsync(id);
            if (e is null) return NotFound();

            // NÃO expõe SenhaHash
            return Ok(new
            {
                e.Id,
                e.IdLoja,
                e.IdFuncionario,
                e.Login,
                e.Email
            });
        }

        // GET: api/UsuariosLoja?idLoja=1&q=joao&page=1&pageSize=50
        [HttpGet]
        public async Task<ActionResult> List(
            [FromQuery] int? idLoja,
            [FromQuery] int? idFuncionario,
            [FromQuery] string? q,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var (items, total) = await _logic.ListAsync(idLoja, idFuncionario, q, page, pageSize);

            // Mapeia a saída sem SenhaHash
            var safe = items.Select(e => new {
                e.Id,
                e.IdLoja,
                e.IdFuncionario,
                e.Login,
                e.Email
            });

            return Ok(new { items = safe, total, page, pageSize });
        }

        // POST: api/UsuariosLoja
        [HttpPost]
        public async Task<ResponseMessage> Create([FromBody] UsuariosLojaEntity body)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            try
            {
                responseMessage = await _logic.CreateAsync(body);

                return responseMessage;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        // PUT: api/UsuariosLoja/5
        // Atualiza dados gerais (NÃO altera senha aqui)
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] UsuariosLojaEntity body, [FromQuery] string? rvBase64 = null)
        {
            if (id != body.Id) return BadRequest(new { message = "Id da rota difere do corpo." });

            byte[]? rv = null;
            if (!string.IsNullOrWhiteSpace(rvBase64))
            {
                try { rv = Convert.FromBase64String(rvBase64); } catch { rv = null; }
            }

            try
            {
                var updated = await _logic.UpdateAsync(body, rv);
                if (updated is null) return NotFound();

                return Ok(new
                {
                    updated.Id,
                    updated.IdLoja,
                    updated.IdFuncionario,
                    updated.Login,
                    updated.Email
                });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("concorrência", StringComparison.OrdinalIgnoreCase))
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE: api/UsuariosLoja/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, [FromQuery] string? rvBase64 = null)
        {
            byte[]? rv = null;
            if (!string.IsNullOrWhiteSpace(rvBase64))
            {
                try { rv = Convert.FromBase64String(rvBase64); } catch { rv = null; }
            }

            try
            {
                var ok = await _logic.DeleteAsync(id, rv);
                if (!ok) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("concorrência", StringComparison.OrdinalIgnoreCase))
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // POST: api/UsuariosLoja/login
        // Envie: { "IdLoja": 1, "Login": "joao", "SenhaHash": "senha_pura_aqui" }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuariosLojaEntity body)
        {
            if (body.IdLoja <= 0 || string.IsNullOrWhiteSpace(body.Login) || string.IsNullOrEmpty(body.Senha))
                return BadRequest(new { message = "IdLoja, Login e Senha são obrigatórios." });

            var user = await _logic.LoginAsync(body.IdLoja, body.Login, body.Senha);
            if (user is null) return Unauthorized(new { message = "Login ou senha inválidos." });

            // Retorna usuário sem SenhaHash (e aqui você poderia emitir JWT)
            return Ok(new { user.Id, user.IdLoja, user.Login, user.Email });
        }

        // OPCIONAL: troca de senha sem DTO (via query/headers ou corpo simples)
        // Ex.: POST api/UsuariosLoja/5/senha?senhaAtual=abc&novaSenha=xyz&rvBase64=...
        [HttpPost("{id:int}/senha")]
        public async Task<ActionResult> ChangePassword(
            int id,
            [FromQuery] string senhaAtual,
            [FromQuery] string novaSenha,
            [FromQuery] string? rvBase64 = null)
        {
            byte[]? rv = null;
            if (!string.IsNullOrWhiteSpace(rvBase64))
            {
                try { rv = Convert.FromBase64String(rvBase64); } catch { rv = null; }
            }

            var ok = await _logic.ChangePasswordAsync(id, senhaAtual, novaSenha, rv);
            return ok ? NoContent() : BadRequest(new { message = "Não foi possível alterar a senha." });
        }
    }
}
