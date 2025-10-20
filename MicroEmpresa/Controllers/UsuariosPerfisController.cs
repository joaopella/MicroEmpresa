using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace MicroEmpresa.Controllers
{
    [ApiController]
    [Route("api/usuarios/{idUsuario:int}/perfis")]
    public class UsuariosPerfisController : ControllerBase
    {
        private readonly IUsuariosPerfisLogic _logic;
        public UsuariosPerfisController(IUsuariosPerfisLogic logic) => _logic = logic;

        // GET /api/usuarios/{idUsuario}/perfis
        [HttpGet]
        public async Task<IActionResult> Listar(int idUsuario)
        {
            var (ok, error, items) = await _logic.ListByUsuarioAsync(idUsuario);
            return ok ? Ok(items) : NotFound(new { error });
        }

        // PUT /api/usuarios/{idUsuario}/perfis/{idPerfil}
        // (idempotente) adiciona o perfil ao usuário
        [HttpPut("{idPerfil:int}")]
        public async Task<IActionResult> Vincular(int idUsuario, int idPerfil)
        {
            var (ok, error, saved) = await _logic.AddAsync(idUsuario, idPerfil);
            return ok ? Ok(saved) : BadRequest(new { error });
        }

        // DELETE /api/usuarios/{idUsuario}/perfis/{idPerfil}
        [HttpDelete("{idPerfil:int}")]
        public async Task<IActionResult> Desvincular(int idUsuario, int idPerfil)
        {
            var (ok, error) = await _logic.RemoveAsync(idUsuario, idPerfil);
            return ok ? NoContent() : NotFound(new { error });
        }
    }
}
