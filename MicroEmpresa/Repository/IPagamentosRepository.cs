using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IPagamentosRepository
    {
        Task<List<PagamentosEntity>> ListarPorVendaAsync(int idVenda);
        Task<PagamentosEntity?> ObterAsync(int id);

        Task<int> CriarAsync(PagamentosEntity entity);
        Task<bool> AtualizarAsync(PagamentosEntity entity); // usa Rv para concorrência
        Task<bool> ExcluirAsync(int id);
    }
}
