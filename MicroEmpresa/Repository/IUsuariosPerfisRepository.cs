using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IUsuariosPerfisRepository
    {
        Task<bool> UsuarioExisteAsync(int idUsuario);
        Task<bool> PerfilExisteAsync(int idPerfil);

        Task<IReadOnlyList<UsuariosPerfisEntity>> ListByUsuarioAsync(int idUsuario);
        Task<IReadOnlyList<UsuariosPerfisEntity>> ListByPerfilAsync(int idPerfil);

        Task<UsuariosPerfisEntity?> GetAsync(int idUsuario, int idPerfil);
        Task<UsuariosPerfisEntity> AddAsync(UsuariosPerfisEntity entity);
        Task<bool> DeleteAsync(int idUsuario, int idPerfil);
    }
}
