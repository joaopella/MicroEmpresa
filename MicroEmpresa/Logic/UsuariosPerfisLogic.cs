using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;

namespace MicroEmpresa.Logic
{
    public class UsuariosPerfisLogic : IUsuariosPerfisLogic
    {
        private readonly IUsuariosPerfisRepository _repo;
        public UsuariosPerfisLogic(IUsuariosPerfisRepository repo) => _repo = repo;

        public async Task<(bool ok, string? error, IReadOnlyList<UsuariosPerfisEntity>? items)>
            ListByUsuarioAsync(int idUsuario)
        {
            if (!await _repo.UsuarioExisteAsync(idUsuario))
                return (false, "Usuário não existe.", null);

            var items = await _repo.ListByUsuarioAsync(idUsuario);
            return (true, null, items);
        }

        public async Task<(bool ok, string? error, UsuariosPerfisEntity? saved)>
            AddAsync(int idUsuario, int idPerfil)
        {
            if (!await _repo.UsuarioExisteAsync(idUsuario))
                return (false, "Usuário inválido.", null);
            if (!await _repo.PerfilExisteAsync(idPerfil))
                return (false, "Perfil inválido.", null);

            var jaExiste = await _repo.GetAsync(idUsuario, idPerfil);
            if (jaExiste is not null)
                return (true, null, jaExiste); // idempotente

            var entity = new UsuariosPerfisEntity
            {
                IdUsuario = idUsuario,
                IdPerfil = idPerfil,
                CriadoEm = DateTime.UtcNow
            };

            var saved = await _repo.AddAsync(entity);
            return (true, null, saved);
        }

        public async Task<(bool ok, string? error)> RemoveAsync(int idUsuario, int idPerfil)
        {
            var ok = await _repo.DeleteAsync(idUsuario, idPerfil);
            return ok ? (true, null) : (false, "Vínculo não encontrado.");
        }
    }
}
