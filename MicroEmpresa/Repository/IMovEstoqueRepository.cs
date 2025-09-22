using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IMovEstoqueRepository
    {
        Task<List<MovEstoqueEntity>> ListarPorLojaProdutoAsync(int idLoja, int idProduto);
        Task<MovEstoqueEntity?> ObterAsync(int id);

        /// <summary>
        /// Cria o movimento e ajusta o saldo do estoque na mesma transação.
        /// Retorna o ID do movimento criado.
        /// </summary>
        Task<int> CriarComAjusteAsync(MovEstoqueEntity mov);

        /// <summary>
        /// Exclui o movimento. Se estornar=true, desfaz o impacto no saldo.
        /// </summary>
        Task<bool> ExcluirAsync(int id, bool estornar);
    }
}
