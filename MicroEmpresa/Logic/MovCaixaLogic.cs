using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Logic
{
    public class MovCaixaLogic : IMovCaixaLogic
    {
        private readonly IMovCaixaRepository _repo;
        public MovCaixaLogic(IMovCaixaRepository repo) => _repo = repo;

        public Task<List<MovCaixaEntity>> ListarPorCaixaAsync(int idCaixa) => _repo.ListarPorCaixaAsync(idCaixa);
        public Task<MovCaixaEntity?> ObterAsync(int id) => _repo.ObterAsync(id);

        public async Task<ResponseMessage> CriarAsync(MovCaixaEntity e)
        {
            if (e.IdCaixa <= 0) return new ResponseMessage { Message = "Caixa inválido." };
            if (string.IsNullOrWhiteSpace(e.Tipo)) return new ResponseMessage { Message = "Tipo obrigatório." };
            if (string.IsNullOrWhiteSpace(e.Origem)) return new ResponseMessage { Message = "Origem obrigatória." };
            if (e.Valor <= 0) return new ResponseMessage { Message = "Valor deve ser maior que zero." };

            if (e.DataMov == default) e.DataMov = DateTime.UtcNow;

            var id = await _repo.CriarAsync(e);
            return new ResponseMessage { Message = "OK", Data = id.ToString() };
        }

        public async Task<ResponseMessage> AtualizarDescricaoAsync(int id, byte[] rv, string? descricao)
        {
            if (id <= 0) return new ResponseMessage { Message = "ID inválido." };
            if (rv is null || rv.Length == 0) return new ResponseMessage { Message = "RowVersion (Rv) é obrigatório." };

            try
            {
                var ok = await _repo.AtualizarDescricaoAsync(id, rv, descricao);
                return ok ? new ResponseMessage { Message = "OK" }
                          : new ResponseMessage { Message = "Movimento não encontrado." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Concorrência detectada." };
            }
        }

        public async Task<ResponseMessage> ExcluirAsync(int id)
        {
            var ok = await _repo.ExcluirAsync(id);
            return ok ? new ResponseMessage { Message = "OK" }
                      : new ResponseMessage { Message = "Movimento não encontrado." };
        }
    }
}
