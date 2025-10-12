using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace MicroEmpresa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasItensController : ControllerBase
    {
        private readonly IVendasItensLogic _logic;
        public VendasItensController(IVendasItensLogic logic) => _logic = logic;

        [HttpGet]
        public Task<List<VendasItensEntity>> Listar() => _logic.ListarAsync();

        [HttpGet("por-venda/{idVenda:int}")]
        public Task<List<VendasItensEntity>> ListarPorVenda(int idVenda) => _logic.ListarPorVendaAsync(idVenda);

        [HttpGet("{id:int}")]
        public Task<VendasItensEntity?> Obter(int id) => _logic.ObterAsync(id);

        [HttpPost]
        public Task<ResponseMessage> Criar([FromBody] VendasItensEntity item) => _logic.CriarAsync(item);

        [HttpPut("{id:int}")]
        public Task<ResponseMessage> Atualizar(int id, [FromBody] VendasItensEntity item)
        {
            item.Id = id;
            return _logic.AtualizarAsync(item);
        }

        [HttpDelete("{id:int}")]
        public Task<ResponseMessage> Remover(int id, [FromQuery] byte[] rv) => _logic.RemoverAsync(id, rv);
    }
}
