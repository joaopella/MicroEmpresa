using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace MicroEmpresa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasController : ControllerBase
    {
        private readonly IVendasLogic _logic;
        public VendasController(IVendasLogic logic) => _logic = logic;

        [HttpGet]
        public Task<List<VendasEntity>> Listar() => _logic.ListarAsync();

        [HttpGet("{id:int}")]
        public Task<VendasEntity?> Obter(int id) => _logic.ObterAsync(id);

        [HttpPost]
        public Task<ResponseMessage> Criar([FromBody] VendasEntity venda) => _logic.CriarAsync(venda);

        [HttpPut("{id:int}")]
        public Task<ResponseMessage> Atualizar(int id, [FromBody] VendasEntity venda)
        {
            venda.Id = id;
            return _logic.AtualizarAsync(venda);
        }

        [HttpDelete("{id:int}")]
        public Task<ResponseMessage> Remover(int id, [FromQuery] byte[] rv) => _logic.RemoverAsync(id, rv);
    }
}
