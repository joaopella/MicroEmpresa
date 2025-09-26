using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IProdutosRepository
    {
        Task<List<ProdutosEntity>> ListarAsync();
        Task<List<ProdutosEntity>> ListarPorLojaAsync(int idLoja);
        Task<ProdutosEntity?> ObterAsync(int id);
        Task<ProdutosEntity?> ObterPorLojaSkuAsync(int idLoja, string sku);

        Task CriarAsync(ProdutosEntity entity);

        /// <summary>Atualiza todos os campos (requer Rv); lança DbUpdateConcurrencyException em conflito.</summary>
        Task<bool> AtualizarAsync(ProdutosEntity entity);

        /// <summary>Atualiza campos parciais respeitando rowversion. Se setLojaSku=false não troca as chaves (IdLoja, Sku).</summary>
        Task<bool> AtualizarCamposAsync(ProdutosEntity entity, bool setLojaSku);

        Task<bool> ExcluirAsync(int id);
    }
}
