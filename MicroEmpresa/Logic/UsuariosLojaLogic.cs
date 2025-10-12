using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;

namespace MicroEmpresa.Logic
{
    public class UsuariosLojaLogic : IUsuariosLojaLogic
    {
        private readonly IUsuariosLojaRepository _repo;

        public UsuariosLojaLogic(IUsuariosLojaRepository repo) => _repo = repo;

        public Task<UsuariosLojaEntity?> GetAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<(IReadOnlyList<UsuariosLojaEntity> Items, int Total)> ListAsync(
            int? idLoja = null, int? idFuncionario = null, string? q = null, int page = 1, int pageSize = 50)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 200);
            var skip = (page - 1) * pageSize;

            var items = await _repo.ListAsync(idLoja, idFuncionario, q, skip, pageSize);
            var total = await _repo.CountAsync(idLoja, idFuncionario, q);
            return (items, total);
        }

        public async Task<ResponseMessage> CreateAsync(UsuariosLojaEntity usuariosLojaEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(usuariosLojaEntity.Login))
                {
                    return new ResponseMessage { Message = "Nome Fantasia deve ter no máximo 150 caracteres." };
                }

                if (string.IsNullOrEmpty(usuariosLojaEntity.Senha))
                {
                    return new ResponseMessage { Message = "CNPJ deve ser preenchido." };
                }

                if (string.IsNullOrEmpty(usuariosLojaEntity.Email))
                {
                    return new ResponseMessage { Message = "CNPJ deve ser preenchido." };
                }

                if (string.IsNullOrEmpty(usuariosLojaEntity.Cnpj))
                {
                    return new ResponseMessage { Message = "CNPJ deve ser preenchido." };
                }

                bool verificaCnpj = await _repo.ExisteCnpjAsync(usuariosLojaEntity.Cnpj);

                if (verificaCnpj)
                {
                    int IdLoja = await _repo.BuscarIdLojaPorCnpjAsync(usuariosLojaEntity.Cnpj);

                    if (IdLoja == 0)
                    {
                        return new ResponseMessage { Message = "CNPJ não encontrado." };
                    }

                    usuariosLojaEntity.IdLoja = IdLoja;
                    usuariosLojaEntity.Login = usuariosLojaEntity.Login.Trim();
                    usuariosLojaEntity.Email = string.IsNullOrWhiteSpace(usuariosLojaEntity.Email) ? null : usuariosLojaEntity.Email!.Trim();
                    usuariosLojaEntity.Senha = ToBase64(usuariosLojaEntity.Senha);
                    usuariosLojaEntity.CriadoEm = DateTime.UtcNow;
                    usuariosLojaEntity.AtualizadoEm = null;

                    var saved = await _repo.AddAsync(usuariosLojaEntity);

                    return new ResponseMessage
                    {
                        Message = "Loja cadastrada com sucesso!",
                    };
                }
                else
                {
                    return new ResponseMessage { Message = "CNPJ inválido." };
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<UsuariosLojaEntity?> UpdateAsync(UsuariosLojaEntity entity, byte[]? rv)
        {
            var atual = await _repo.GetByIdAsync(entity.Id);
            if (atual is null) return null;

            if (rv is { Length: > 0 } && (atual.Rv is null || !atual.Rv.SequenceEqual(rv)))
                throw new InvalidOperationException("Registro desatualizado (concorrência).");

            if (!string.IsNullOrWhiteSpace(entity.Login)) atual.Login = entity.Login.Trim();
            atual.Email = entity.Email is null || string.IsNullOrWhiteSpace(entity.Email) ? null : entity.Email.Trim();
            atual.IdLoja = entity.IdLoja;
            atual.IdFuncionario = entity.IdFuncionario;
            atual.AtualizadoEm = DateTime.UtcNow;

            await _repo.UpdateAsync(atual);
            return atual;
        }

        public async Task<bool> DeleteAsync(int id, byte[]? rv)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e is null) return false;

            if (rv is { Length: > 0 } && (e.Rv is null || !e.Rv.SequenceEqual(rv)))
                throw new InvalidOperationException("Registro desatualizado (concorrência).");

            await _repo.DeleteAsync(e);
            return true;
        }

        public async Task<UsuariosLojaEntity?> LoginAsync(int idLoja, string login, string senhaPura)
        {
            var e = await _repo.GetByLoginAsync(idLoja, login.Trim());
            if (e is null) return null;

            return null;
        }

        public async Task<bool> ChangePasswordAsync(int id, string senhaAtual, string novaSenha, byte[]? rv)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e is null) return false;

            if (rv is { Length: > 0 } && (e.Rv is null || !e.Rv.SequenceEqual(rv)))
                throw new InvalidOperationException("Registro desatualizado (concorrência).");

            
            return false;
        }

        public static string ToBase64(string plainText)
        {
            if (plainText == null) return string.Empty;
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
        }
    }
}
