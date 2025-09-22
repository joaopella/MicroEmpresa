using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IMovEstoqueLogic
    {
        Task<List<MovEstoqueEntity>> ListarPorLojaProdutoAsync(int idLoja, int idProduto);
        Task<MovEstoqueEntity?> ObterAsync(int id);

        Task<ResponseMessage> CriarAsync(MovEstoqueEntity entity);
        Task<ResponseMessage> ExcluirAsync(int id, bool estornar);
    }
}
