using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IEstoquesRepository
    {
        Task<List<EstoquesEntity>> ListarAsync();
        Task<EstoquesEntity?> ObterAsync(int id);
        Task<EstoquesEntity?> ObterPorLojaProdutoAsync(int idLoja, int idProduto);

        Task CriarAsync(EstoquesEntity entity);

        /// <summary>Atualiza todos os campos (requer Rv); lança DbUpdateConcurrencyException em conflito.</summary>
        Task<bool> AtualizarAsync(EstoquesEntity entity);

        /// <summary>Atualiza campos parciais respeitando rowversion. Se setLojaProduto=false não troca as chaves.</summary>
        Task<bool> AtualizarCamposAsync(EstoquesEntity entity, bool setLojaProduto);

        Task<bool> ExcluirAsync(int id);
    }
}
