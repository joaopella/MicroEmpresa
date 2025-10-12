using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IVendasLogic
    {
        Task<List<VendasEntity>> ListarAsync();
        Task<VendasEntity?> ObterAsync(int id);
        Task<ResponseMessage> CriarAsync(VendasEntity venda);
        Task<ResponseMessage> AtualizarAsync(VendasEntity venda);
        Task<ResponseMessage> RemoverAsync(int id, byte[] rv);
    }
}
