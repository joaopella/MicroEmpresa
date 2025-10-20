using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class UsuariosPerfisData : IUsuariosPerfisRepository
    {
        private readonly AppDbContext _db;
        public UsuariosPerfisData(AppDbContext db) => _db = db;

        public Task<bool> UsuarioExisteAsync(int idUsuario) =>
            _db.Set<UsuariosLojaEntity>().AnyAsync(u => u.Id == idUsuario);

        public Task<bool> PerfilExisteAsync(int idPerfil) =>
            _db.Set<PerfisEntity>().AnyAsync(p => p.Id == idPerfil);

        public async Task<IReadOnlyList<UsuariosPerfisEntity>> ListByUsuarioAsync(int idUsuario) =>
            await _db.Set<UsuariosPerfisEntity>()
                     .AsNoTracking()
                     .Where(x => x.IdUsuario == idUsuario)
                     .OrderBy(x => x.IdPerfil)
                     .ToListAsync();

        public async Task<IReadOnlyList<UsuariosPerfisEntity>> ListByPerfilAsync(int idPerfil) =>
            await _db.Set<UsuariosPerfisEntity>()
                     .AsNoTracking()
                     .Where(x => x.IdPerfil == idPerfil)
                     .OrderBy(x => x.IdUsuario)
                     .ToListAsync();

        public Task<UsuariosPerfisEntity?> GetAsync(int idUsuario, int idPerfil) =>
            _db.Set<UsuariosPerfisEntity>()
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.IdUsuario == idUsuario && x.IdPerfil == idPerfil);

        public async Task<UsuariosPerfisEntity> AddAsync(UsuariosPerfisEntity entity)
        {
            _db.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int idUsuario, int idPerfil)
        {
            var link = await _db.Set<UsuariosPerfisEntity>()
                                .FirstOrDefaultAsync(x => x.IdUsuario == idUsuario && x.IdPerfil == idPerfil);
            if (link is null) return false;
            _db.Remove(link);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
