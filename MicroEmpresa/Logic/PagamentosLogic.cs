using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Logic
{
    public class PagamentosLogic : IPagamentosLogic
    {
        private static readonly HashSet<string> _formasValidas =
            new(StringComparer.OrdinalIgnoreCase) { "dinheiro", "credito", "debito", "pix", "boleto", "voucher" };

        private readonly IPagamentosRepository _repo;
        public PagamentosLogic(IPagamentosRepository repo) => _repo = repo;

        public Task<List<PagamentosEntity>> ListarPorVendaAsync(int idVenda) => _repo.ListarPorVendaAsync(idVenda);
        public Task<PagamentosEntity?> ObterAsync(int id) => _repo.ObterAsync(id);

        public async Task<ResponseMessage> CriarAsync(PagamentosEntity e)
        {
            if (e.IdVenda <= 0) return new ResponseMessage { Message = "Venda inválida." };
            if (string.IsNullOrWhiteSpace(e.FormaPagamento)) return new ResponseMessage { Message = "Forma de pagamento obrigatória." };
            if (e.Valor <= 0) return new ResponseMessage { Message = "Valor deve ser maior que zero." };

            // valida forma (pode remover se quiser deixar livre)
            if (!_formasValidas.Contains(e.FormaPagamento))
                return new ResponseMessage { Message = "Forma de pagamento inválida." };

            var id = await _repo.CriarAsync(e);
            return new ResponseMessage { Message = "OK", Data = id.ToString() };
        }

        public async Task<ResponseMessage> AtualizarAsync(PagamentosEntity e)
        {
            if (e.Id <= 0) return new ResponseMessage { Message = "ID inválido." };
            if (e.Rv is null || e.Rv.Length == 0) return new ResponseMessage { Message = "RowVersion (Rv) é obrigatório." };
            if (!string.IsNullOrWhiteSpace(e.FormaPagamento) && !_formasValidas.Contains(e.FormaPagamento))
                return new ResponseMessage { Message = "Forma de pagamento inválida." };
            if (e.Valor <= 0) return new ResponseMessage { Message = "Valor deve ser maior que zero." };

            try
            {
                var ok = await _repo.AtualizarAsync(e);
                return ok ? new ResponseMessage { Message = "OK" }
                          : new ResponseMessage { Message = "Pagamento não encontrado." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Concorrência detectada. Atualize e tente novamente." };
            }
        }

        public async Task<ResponseMessage> ExcluirAsync(int id)
        {
            var ok = await _repo.ExcluirAsync(id);
            return ok ? new ResponseMessage { Message = "OK" }
                      : new ResponseMessage { Message = "Pagamento não encontrado." };
        }
    }
}
