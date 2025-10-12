using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Logic
{
    public class VendasLogic : IVendasLogic
    {
        private readonly IVendasRepository _repo;
        public VendasLogic(IVendasRepository repo) => _repo = repo;

        public Task<List<VendasEntity>> ListarAsync() => _repo.ListarAsync();

        public Task<VendasEntity?> ObterAsync(int id) => _repo.ObterAsync(id);

        public async Task<ResponseMessage> CriarAsync(VendasEntity v)
        {
            // validações simples no padrão que você pediu
            if (v.IdLoja <= 0)
                return new ResponseMessage { Message = "Id da Loja é obrigatório." };

            if (v.DataVenda == default)
                return new ResponseMessage { Message = "Data da venda é obrigatória." };

            if (v.DescontoTotal is < 0)
                return new ResponseMessage { Message = "Desconto Total não pode ser negativo." };

            if (v.AcrescimoTotal is < 0)
                return new ResponseMessage { Message = "Acréscimo Total não pode ser negativo." };

            v.CriadoEm = DateTime.Now;
            v.AtualizadoEm = null;

            await _repo.CriarAsync(v);
            return new ResponseMessage { Message = "Venda criada com sucesso." };
        }

        public async Task<ResponseMessage> AtualizarAsync(VendasEntity v)
        {
            if (v.Id <= 0)
                return new ResponseMessage { Message = "ID inválido." };

            if (v.Rv == null || v.Rv.Length == 0)
                return new ResponseMessage { Message = "RV (rowversion) é obrigatório para atualizar." };

            if (v.IdLoja <= 0)
                return new ResponseMessage { Message = "Id da Loja é obrigatório." };

            if (v.DataVenda == default)
                return new ResponseMessage { Message = "Data da venda é obrigatória." };

            if (v.DescontoTotal is < 0)
                return new ResponseMessage { Message = "Desconto Total não pode ser negativo." };

            if (v.AcrescimoTotal is < 0)
                return new ResponseMessage { Message = "Acréscimo Total não pode ser negativo." };

            v.AtualizadoEm = DateTime.Now;

            try
            {
                await _repo.AtualizarAsync(v);
                return new ResponseMessage { Message = "Venda atualizada com sucesso." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Registro desatualizado: conflito de RV (alguém já alterou este registro)." };
            }
        }

        public async Task<ResponseMessage> RemoverAsync(int id, byte[] rv)
        {
            if (id <= 0)
                return new ResponseMessage { Message = "ID inválido." };

            if (rv == null || rv.Length == 0)
                return new ResponseMessage { Message = "RV (rowversion) é obrigatório para remover." };

            try
            {
                await _repo.RemoverAsync(id, rv);
                return new ResponseMessage { Message = "Venda removida com sucesso." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Registro desatualizado: conflito de RV ao remover." };
            }
        }
    }
}
