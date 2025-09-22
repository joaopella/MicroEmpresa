using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IEstoquesLogic
    {
        Task<List<EstoquesEntity>> ListarAsync();
        Task<EstoquesEntity?> ObterAsync(int id);
        Task<EstoquesEntity?> ObterPorLojaProdutoAsync(int idLoja, int idProduto);

        Task<ResponseMessage> CriarAsync(EstoquesEntity entity);
        Task<ResponseMessage> AtualizarAsync(EstoquesEntity entity);
        Task<ResponseMessage> AjustarSaldoAsync(int id, decimal delta, byte[] rv);
        Task<ResponseMessage> ExcluirAsync(int id);
    }
}
