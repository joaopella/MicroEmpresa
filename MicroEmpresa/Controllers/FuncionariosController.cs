using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace MicroEmpresa.Controllers
{
    [ApiController]
    [Route("api/funcionarios")]
    public class FuncionariosController : ControllerBase
    {
        private readonly IFuncionariosLogic _svc;
        public FuncionariosController(IFuncionariosLogic svc) => _svc = svc;

        [HttpGet]
        public async Task<ActionResult<List<FuncionariosEntity>>> Listar() =>
            Ok(await _svc.ListarAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Obter(int id)
        {
            var f = await _svc.ObterAsync(id);
            return Ok(f);
        }

        [HttpPost]
        public async Task<ResponseMessage> Criar([FromBody] FuncionariosEntity funcionariosEntity)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                responseMessage = await _svc.CriarAsync(funcionariosEntity);

                return responseMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ResponseMessage>> Atualizar(int id, [FromBody] FuncionariosEntity entity)
        {
            var ok = await _svc.AtualizarAsync(id, entity);
            return Ok(new { message =  "Funcionário atualizado com sucesso!"  });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Remover(int id)
        {
            await _svc.RemoverAsync(id);
            return Ok(new { message = "Funcionário removido com sucesso!" });
        }
    }
}
