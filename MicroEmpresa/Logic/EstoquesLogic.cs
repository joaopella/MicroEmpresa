using MicroEmpresa.Date;
using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Logic
{
    public class EstoquesLogic : IEstoquesLogic
    {
        private readonly EstoquesData _data;

        public EstoquesLogic(EstoquesData data) => _data = data;

        public Task<List<EstoquesEntity>> ListarAsync() => _data.ListarAsync();

        public Task<EstoquesEntity?> ObterAsync(int id) => _data.ObterAsync(id);

        public Task<EstoquesEntity?> ObterPorLojaProdutoAsync(int idLoja, int idProduto)
            => _data.ObterPorLojaProdutoAsync(idLoja, idProduto);

        public async Task<ResponseMessage> CriarAsync(EstoquesEntity e)
        {
            // validações
            if (e.IdLoja <= 0) return new ResponseMessage { Message = "IdLoja inválido." };
            if (e.IdProduto <= 0) return new ResponseMessage { Message = "IdProduto inválido." };
            if (e.Saldo < 0) return new ResponseMessage { Message = "Saldo não pode ser negativo." };

            // um estoque por (Loja, Produto)
            var existente = await _data.ObterPorLojaProdutoAsync(e.IdLoja, e.IdProduto);
            if (existente is not null) return new ResponseMessage { Message = "Já existe estoque para essa Loja/Produto." };

            await _data.CriarAsync(e);
            return new ResponseMessage { Message = "OK" };
        }

        public async Task<ResponseMessage> AtualizarAsync(EstoquesEntity e)
        {
            if (e.Id <= 0) return new ResponseMessage { Message = "ID inválido." };
            if (e.IdLoja <= 0) return new ResponseMessage { Message = "IdLoja inválido." };
            if (e.IdProduto <= 0) return new ResponseMessage { Message = "IdProduto inválido." };
            if (e.Saldo < 0) return new ResponseMessage { Message = "Saldo não pode ser negativo." };
            if (e.Rv is null || e.Rv.Length == 0) return new ResponseMessage { Message = "RowVersion (Rv) é obrigatório para atualizar." };

            try
            {
                var ok = await _data.AtualizarAsync(e);
                return ok ? new ResponseMessage { Message = "OK" }
                          : new ResponseMessage { Message = "Estoque não encontrado." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Concorrência detectada. O registro foi alterado por outro usuário." };
            }
        }

        public async Task<ResponseMessage> AjustarSaldoAsync(int id, decimal delta, byte[] rv)
        {
            if (id <= 0) return new ResponseMessage { Message = "ID inválido." };
            if (rv is null || rv.Length == 0) return new ResponseMessage { Message = "RowVersion (Rv) é obrigatório." };

            var e = await _data.ObterAsync(id);
            if (e is null) return new ResponseMessage { Message = "Estoque não encontrado." };

            var novoSaldo = e.Saldo + delta;
            if (novoSaldo < 0) return new ResponseMessage { Message = "Saldo resultante não pode ser negativo." };

            // aplica o Rv recebido para respeitar a concorrência
            e.Rv = rv;
            e.Saldo = novoSaldo;

            try
            {
                var ok = await _data.AtualizarCamposAsync(e, setLojaProduto: false);
                return ok ? new ResponseMessage { Message = "OK" }
                          : new ResponseMessage { Message = "Estoque não encontrado." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Concorrência detectada. Atualize a página e tente novamente." };
            }
        }

        public async Task<ResponseMessage> ExcluirAsync(int id)
        {
            var ok = await _data.ExcluirAsync(id);
            return ok ? new ResponseMessage { Message = "OK" }
                      : new ResponseMessage { Message = "Estoque não encontrado." };
        }
    }
}
