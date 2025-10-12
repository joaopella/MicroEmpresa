using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IVendasItensLogic
    {
        Task<List<VendasItensEntity>> ListarAsync();
        Task<List<VendasItensEntity>> ListarPorVendaAsync(int idVenda);
        Task<VendasItensEntity?> ObterAsync(int id);
        Task<ResponseMessage> CriarAsync(VendasItensEntity item);
        Task<ResponseMessage> AtualizarAsync(VendasItensEntity item);
        Task<ResponseMessage> RemoverAsync(int id, byte[] rv);
    }
}
