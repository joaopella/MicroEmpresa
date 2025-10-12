using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IVendasItensRepository
    {
        Task<List<VendasItensEntity>> ListarAsync();
        Task<List<VendasItensEntity>> ListarPorVendaAsync(int idVenda);
        Task<VendasItensEntity?> ObterAsync(int id);
        Task<int> CriarAsync(VendasItensEntity item);
        Task<int> AtualizarAsync(VendasItensEntity item);
        Task<int> RemoverAsync(int id, byte[] rv);
    }
}
