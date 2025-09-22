using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;

namespace MicroEmpresa.Logic
{
    public class MovEstoqueLogic : IMovEstoqueLogic
    {
        private readonly IMovEstoqueRepository _repo;
        public MovEstoqueLogic(IMovEstoqueRepository repo) => _repo = repo;

        public Task<List<MovEstoqueEntity>> ListarPorLojaProdutoAsync(int idLoja, int idProduto)
            => _repo.ListarPorLojaProdutoAsync(idLoja, idProduto);

        public Task<MovEstoqueEntity?> ObterAsync(int id) => _repo.ObterAsync(id);

        public async Task<ResponseMessage> CriarAsync(MovEstoqueEntity e)
        {
            if (e.IdLoja <= 0) return new ResponseMessage { Message = "IdLoja inválido." };
            if (e.IdProduto <= 0) return new ResponseMessage { Message = "IdProduto inválido." };
            if (string.IsNullOrWhiteSpace(e.Tipo)) return new ResponseMessage { Message = "Tipo obrigatório (entrada/saida)." };
            if (e.Qtd <= 0) return new ResponseMessage { Message = "Quantidade deve ser > 0." };

            e.Tipo = e.Tipo.Trim().ToLowerInvariant();
            if (e.Tipo != "entrada" && e.Tipo != "saida")
                return new ResponseMessage { Message = "Tipo inválido. Use 'entrada' ou 'saida'." };

            try
            {
                var id = await _repo.CriarComAjusteAsync(e);
                return new ResponseMessage { Message = "OK", Data = id.ToString() };
            }
            catch (InvalidOperationException ex)
            {
                return new ResponseMessage { Message = ex.Message };
            }
        }

        public async Task<ResponseMessage> ExcluirAsync(int id, bool estornar)
        {
            try
            {
                var ok = await _repo.ExcluirAsync(id, estornar);
                return ok ? new ResponseMessage { Message = "OK" }
                          : new ResponseMessage { Message = "Movimento não encontrado." };
            }
            catch (InvalidOperationException ex)
            {
                return new ResponseMessage { Message = ex.Message };
            }
        }
    }
}
