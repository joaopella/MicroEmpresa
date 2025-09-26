using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IProdutosLogic
    {
        Task<List<ProdutosEntity>> ListarAsync();
        Task<List<ProdutosEntity>> ListarPorLojaAsync(int idLoja);
        Task<ProdutosEntity?> ObterAsync(int id);
        Task<ProdutosEntity?> ObterPorLojaSkuAsync(int idLoja, string sku);

        Task<ResponseMessage> CriarAsync(ProdutosEntity entity);
        Task<ResponseMessage> AtualizarAsync(ProdutosEntity entity);
        Task<ResponseMessage> AtualizarPrecosAsync(int id, decimal? precoVenda, decimal? custo, decimal? markupPercentual, byte[] rv);
        Task<ResponseMessage> ExcluirAsync(int id);
    }
}
