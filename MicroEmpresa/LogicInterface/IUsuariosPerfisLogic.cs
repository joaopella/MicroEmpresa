using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IUsuariosPerfisLogic
    {
        Task<(bool ok, string? error, IReadOnlyList<UsuariosPerfisEntity>? items)> ListByUsuarioAsync(int idUsuario);
        Task<(bool ok, string? error, UsuariosPerfisEntity? saved)> AddAsync(int idUsuario, int idPerfil);
        Task<(bool ok, string? error)> RemoveAsync(int idUsuario, int idPerfil);
    }
}
