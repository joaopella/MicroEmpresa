using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IPagamentosLogic
    {
        Task<List<PagamentosEntity>> ListarPorVendaAsync(int idVenda);
        Task<PagamentosEntity?> ObterAsync(int id);

        Task<ResponseMessage> CriarAsync(PagamentosEntity entity);
        Task<ResponseMessage> AtualizarAsync(PagamentosEntity entity);
        Task<ResponseMessage> ExcluirAsync(int id);
    }
}
