using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IVendasRepository
    {
        Task<List<VendasEntity>> ListarAsync();
        Task<VendasEntity?> ObterAsync(int id);
        Task<int> CriarAsync(VendasEntity venda);
        Task<int> AtualizarAsync(VendasEntity venda);
        Task<int> RemoverAsync(int id, byte[] rv);
    }
}
