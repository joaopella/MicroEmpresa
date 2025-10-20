using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class FuncionalidadesData : IFuncionalidadesRepository
    {
        private readonly AppDbContext _db;
        public FuncionalidadesData(AppDbContext db) => _db = db;

        public Task<FuncionalidadesEntity?> GetByIdAsync(int id) =>
            _db.Set<FuncionalidadesEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IReadOnlyList<FuncionalidadesEntity>> GetAllAsync(int skip, int take, string? search)
        {
            var q = _db.Set<FuncionalidadesEntity>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(x => x.Descricao.Contains(s));
            }

            return await q.OrderBy(x => x.Descricao)
                          .Skip(skip)
                          .Take(take)
                          .ToListAsync();
        }

        public Task<int> CountAsync(string? search)
        {
            var q = _db.Set<FuncionalidadesEntity>().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(x => x.Descricao.Contains(s));
            }
            return q.CountAsync();
        }

        public Task<bool> ExistsByDescricaoAsync(string descricao, int? ignoreId = null)
        {
            var q = _db.Set<FuncionalidadesEntity>().Where(x => x.Descricao == descricao);
            if (ignoreId.HasValue) q = q.Where(x => x.Id != ignoreId.Value);
            return q.AnyAsync();
        }

        public async Task<FuncionalidadesEntity> AddAsync(FuncionalidadesEntity entity)
        {
            _db.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<FuncionalidadesEntity> UpdateAsync(FuncionalidadesEntity entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var found = await _db.Set<FuncionalidadesEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (found is null) return false;

            _db.Remove(found);
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                // provavel FK em perfis_funcionalidades
                throw;
            }
        }
    }
}
