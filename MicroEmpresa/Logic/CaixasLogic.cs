using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Logic
{
    public class CaixasLogic : ICaixasLogic
    {
        private readonly ICaixasRepository _repo;
        public CaixasLogic(ICaixasRepository repo) => _repo = repo;

        public Task<List<CaixasEntity>> ListarAsync() => _repo.ListarAsync();
        public Task<CaixasEntity?> ObterAsync(int id) => _repo.ObterAsync(id);
        public Task<CaixasEntity?> ObterAbertoPorLojaAsync(int idLoja) => _repo.ObterAbertoPorLojaAsync(idLoja);

        public async Task<ResponseMessage> AbrirAsync(CaixasEntity e)
        {
            // validações
            if (e.IdLoja <= 0) return new ResponseMessage { Message = "IdLoja inválido." };
            if (e.IdFuncionarioAbertura <= 0) return new ResponseMessage { Message = "Funcionário de abertura inválido." };
            if (e.ValorInicial < 0) return new ResponseMessage { Message = "Valor inicial não pode ser negativo." };
            if (e.DataAbertura == default) e.DataAbertura = DateTime.UtcNow;

            // regra: 1 caixa aberto por loja
            var aberto = await _repo.ObterAbertoPorLojaAsync(e.IdLoja);
            if (aberto is not null) return new ResponseMessage { Message = "Já existe um caixa aberto para esta loja." };

            var id = await _repo.AbrirAsync(e);
            return new ResponseMessage { Message = "OK", Data = id.ToString() };
        }

        public async Task<ResponseMessage> FecharAsync(int id, byte[] rv, int idFuncionarioFechamento, decimal valorFechamento, DateTime? dataFechamento = null)
        {
            if (id <= 0) return new ResponseMessage { Message = "ID inválido." };
            if (rv is null || rv.Length == 0) return new ResponseMessage { Message = "RowVersion (Rv) é obrigatório." };
            if (idFuncionarioFechamento <= 0) return new ResponseMessage { Message = "Funcionário de fechamento inválido." };
            if (valorFechamento < 0) return new ResponseMessage { Message = "Valor de fechamento inválido." };

            var atual = await _repo.ObterAsync(id);
            if (atual is null) return new ResponseMessage { Message = "Caixa não encontrado." };
            if (atual.DataFechamento is not null) return new ResponseMessage { Message = "Caixa já está fechado." };

            try
            {
                var ok = await _repo.FecharAsync(id, rv, idFuncionarioFechamento, valorFechamento, dataFechamento ?? DateTime.UtcNow);
                return ok ? new ResponseMessage { Message = "OK" }
                          : new ResponseMessage { Message = "Caixa não encontrado." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Concorrência detectada. Atualize os dados e tente novamente." };
            }
        }

        public async Task<ResponseMessage> AtualizarObsAsync(int id, byte[] rv, string? obs)
        {
            if (id <= 0) return new ResponseMessage { Message = "ID inválido." };
            if (rv is null || rv.Length == 0) return new ResponseMessage { Message = "RowVersion (Rv) é obrigatório." };

            try
            {
                var ok = await _repo.AtualizarObsAsync(id, rv, obs);
                return ok ? new ResponseMessage { Message = "OK" }
                          : new ResponseMessage { Message = "Caixa não encontrado." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Concorrência detectada." };
            }
        }

        public async Task<ResponseMessage> ExcluirAsync(int id)
        {
            var c = await _repo.ObterAsync(id);
            if (c is null) return new ResponseMessage { Message = "Caixa não encontrado." };
            if (c.DataFechamento == null) return new ResponseMessage { Message = "Não é possível excluir um caixa aberto." };

            var ok = await _repo.ExcluirAsync(id);
            return ok ? new ResponseMessage { Message = "OK" }
                      : new ResponseMessage { Message = "Caixa não encontrado." };
        }
    }
}
