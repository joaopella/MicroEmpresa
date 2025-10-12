using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Logic
{
    public class VendasItensLogic : IVendasItensLogic
    {
        private readonly IVendasItensRepository _repo;
        public VendasItensLogic(IVendasItensRepository repo) => _repo = repo;

        public Task<List<VendasItensEntity>> ListarAsync() => _repo.ListarAsync();
        public Task<List<VendasItensEntity>> ListarPorVendaAsync(int idVenda) => _repo.ListarPorVendaAsync(idVenda);
        public Task<VendasItensEntity?> ObterAsync(int id) => _repo.ObterAsync(id);

        public async Task<ResponseMessage> CriarAsync(VendasItensEntity i)
        {
            if (i.IdVenda <= 0) return new ResponseMessage { Message = "Id da venda é obrigatório." };
            if (i.IdProduto <= 0) return new ResponseMessage { Message = "Id do produto é obrigatório." };
            if (i.Quantidade <= 0) return new ResponseMessage { Message = "Quantidade deve ser > 0." };
            if (i.PrecoUnit < 0) return new ResponseMessage { Message = "Preço unitário não pode ser negativo." };
            if (i.DescontoItem is < 0) return new ResponseMessage { Message = "Desconto não pode ser negativo." };
            if (i.AcrescimoItem is < 0) return new ResponseMessage { Message = "Acréscimo não pode ser negativo." };

            await _repo.CriarAsync(i);
            return new ResponseMessage { Message = "Item adicionado com sucesso." };
        }

        public async Task<ResponseMessage> AtualizarAsync(VendasItensEntity i)
        {
            if (i.Id <= 0) return new ResponseMessage { Message = "ID inválido." };
            if (i.Rv == null || i.Rv.Length == 0) return new ResponseMessage { Message = "RV é obrigatório para atualizar." };
            if (i.IdVenda <= 0) return new ResponseMessage { Message = "Id da venda é obrigatório." };
            if (i.IdProduto <= 0) return new ResponseMessage { Message = "Id do produto é obrigatório." };
            if (i.Quantidade <= 0) return new ResponseMessage { Message = "Quantidade deve ser > 0." };
            if (i.PrecoUnit < 0) return new ResponseMessage { Message = "Preço unitário não pode ser negativo." };
            if (i.DescontoItem is < 0) return new ResponseMessage { Message = "Desconto não pode ser negativo." };
            if (i.AcrescimoItem is < 0) return new ResponseMessage { Message = "Acréscimo não pode ser negativo." };

            try
            {
                await _repo.AtualizarAsync(i);
                return new ResponseMessage { Message = "Item atualizado com sucesso." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Conflito de RV: o registro foi alterado por outro usuário." };
            }
        }

        public async Task<ResponseMessage> RemoverAsync(int id, byte[] rv)
        {
            if (id <= 0) return new ResponseMessage { Message = "ID inválido." };
            if (rv == null || rv.Length == 0) return new ResponseMessage { Message = "RV é obrigatório para remover." };

            try
            {
                await _repo.RemoverAsync(id, rv);
                return new ResponseMessage { Message = "Item removido com sucesso." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Conflito de RV ao remover." };
            }
        }
    }
}
