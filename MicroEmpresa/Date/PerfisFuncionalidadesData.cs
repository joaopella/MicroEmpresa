using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class PerfisFuncionalidadesData : IPerfisFuncionalidadesRepository
    {
        private readonly AppDbContext _db;
        public PerfisFuncionalidadesData(AppDbContext db) => _db = db;

        public Task<PerfisFuncionalidadesEntity?> GetAsync(int idPerfil, int idFuncao) =>
            _db.Set<PerfisFuncionalidadesEntity>()
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.IdPerfil == idPerfil && x.IdFuncao == idFuncao);

        public async Task<IReadOnlyList<PerfisFuncionalidadesEntity>> ListByPerfilAsync(int idPerfil) =>
            await _db.Set<PerfisFuncionalidadesEntity>()
                     .AsNoTracking()
                     .Where(x => x.IdPerfil == idPerfil)
                     .OrderBy(x => x.IdFuncao)
                     .ToListAsync();

        public async Task<IReadOnlyList<PerfisFuncionalidadesEntity>> ListByFuncaoAsync(int idFuncao) =>
            await _db.Set<PerfisFuncionalidadesEntity>()
                     .AsNoTracking()
                     .Where(x => x.IdFuncao == idFuncao)
                     .OrderBy(x => x.IdPerfil)
                     .ToListAsync();

        public async Task<PerfisFuncionalidadesEntity> AddAsync(PerfisFuncionalidadesEntity entity)
        {
            _db.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<PerfisFuncionalidadesEntity> UpdateAsync(PerfisFuncionalidadesEntity entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int idPerfil, int idFuncao)
        {
            var found = await _db.Set<PerfisFuncionalidadesEntity>()
                                 .FirstOrDefaultAsync(x => x.IdPerfil == idPerfil && x.IdFuncao == idFuncao);
            if (found is null) return false;
            _db.Remove(found);
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<bool> PerfilExisteAsync(int idPerfil) =>
            _db.Set<PerfisEntity>().AnyAsync(p => p.Id == idPerfil);

        public Task<bool> FuncaoExisteAsync(int idFuncao) =>
            _db.Set<FuncionalidadesEntity>().AnyAsync(f => f.Id == idFuncao);
    }
}
