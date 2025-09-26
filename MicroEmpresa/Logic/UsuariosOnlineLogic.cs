using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Logic
{
    public class UsuariosOnlineLogic : IUsuariosOnlineLogic
    {
        private readonly IUsuariosOnlineRepository _repo;
        public UsuariosOnlineLogic(IUsuariosOnlineRepository repo) => _repo = repo;

        public Task<List<UsuariosOnlineEntity>> ListarAsync() => _repo.ListarAsync();
        public Task<List<UsuariosOnlineEntity>> ListarPorLojaAsync(int idLoja) => _repo.ListarPorLojaAsync(idLoja);
        public Task<UsuariosOnlineEntity?> ObterAsync(int id) => _repo.ObterAsync(id);
        public Task<UsuariosOnlineEntity?> ObterPorLojaLoginAsync(int idLoja, string login) => _repo.ObterPorLojaLoginAsync(idLoja, login);

        public async Task<ResponseMessage> CriarAsync(UsuariosOnlineEntity e)
        {
            if (e.IdLoja <= 0) return new ResponseMessage { Message = "IdLoja inválido." };
            if (string.IsNullOrWhiteSpace(e.Login)) return new ResponseMessage { Message = "Login é obrigatório." };
            if (e.SenhaHash is null || e.SenhaHash.Length == 0) return new ResponseMessage { Message = "SenhaHash é obrigatório." };

            var dup = await _repo.ObterPorLojaLoginAsync(e.IdLoja, e.Login);
            if (dup is not null) return new ResponseMessage { Message = "Já existe usuário com esse login nesta loja." };

            e.CriadoEm = DateTime.UtcNow;
            e.AtualizadoEm = DateTime.UtcNow;

            await _repo.CriarAsync(e);
            return new ResponseMessage { Message = "OK" };
        }

        public async Task<ResponseMessage> AtualizarAsync(UsuariosOnlineEntity e)
        {
            if (e.Id <= 0) return new ResponseMessage { Message = "ID inválido." };
            if (e.IdLoja <= 0) return new ResponseMessage { Message = "IdLoja inválido." };
            if (string.IsNullOrWhiteSpace(e.Login)) return new ResponseMessage { Message = "Login é obrigatório." };
            if (e.Rv is null || e.Rv.Length == 0) return new ResponseMessage { Message = "RowVersion (Rv) é obrigatório." };

            var dup = await _repo.ObterPorLojaLoginAsync(e.IdLoja, e.Login);
            if (dup is not null && dup.Id != e.Id)
                return new ResponseMessage { Message = "Já existe usuário com esse login nesta loja." };

            try
            {
                var ok = await _repo.AtualizarAsync(e);
                return ok ? new ResponseMessage { Message = "OK" }
                          : new ResponseMessage { Message = "Usuário não encontrado." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Concorrência detectada. O registro foi alterado por outro usuário." };
            }
        }

        public async Task<ResponseMessage> AtualizarSenhaAsync(int id, byte[] novaHash, byte[] rv)
        {
            if (id <= 0) return new ResponseMessage { Message = "ID inválido." };
            if (novaHash is null || novaHash.Length == 0) return new ResponseMessage { Message = "SenhaHash é obrigatório." };
            if (rv is null || rv.Length == 0) return new ResponseMessage { Message = "RowVersion (Rv) é obrigatório." };

            try
            {
                var ok = await _repo.AtualizarSenhaAsync(id, novaHash, rv);
                return ok ? new ResponseMessage { Message = "OK" }
                          : new ResponseMessage { Message = "Usuário não encontrado." };
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
                      : new ResponseMessage { Message = "Usuário não encontrado." };
        }
    }
}
