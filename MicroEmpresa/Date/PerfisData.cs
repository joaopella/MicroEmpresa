using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class PerfisData : IPerfisRepository
    {
        private readonly AppDbContext _db;
        public PerfisData(AppDbContext db) => _db = db;

        public Task<PerfisEntity?> GetByIdAsync(int id) =>
            _db.Set<PerfisEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IReadOnlyList<PerfisEntity>> GetAllAsync(int skip, int take, string? search)
        {
            var q = _db.Set<PerfisEntity>().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                q = q.Where(x => x.Nome.Contains(search) || (x.Descricao ?? "").Contains(search));
            }
            return await q.OrderBy(x => x.Nome).Skip(skip).Take(take).ToListAsync();
        }

        public Task<int> CountAsync(string? search)
        {
            var q = _db.Set<PerfisEntity>().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                q = q.Where(x => x.Nome.Contains(search) || (x.Descricao ?? "").Contains(search));
            }
            return q.CountAsync();
        }

        public async Task<PerfisEntity> AddAsync(PerfisEntity entity)
        {
            _db.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<PerfisEntity> UpdateAsync(PerfisEntity entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var found = await _db.Set<PerfisEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (found is null) return false;
            _db.Remove(found);
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<bool> ExistsByNameAsync(string nome, int? ignoreId = null)
        {
            var q = _db.Set<PerfisEntity>().Where(x => x.Nome == nome);
            if (ignoreId.HasValue) q = q.Where(x => x.Id != ignoreId.Value);
            return q.AnyAsync();
        }
    }
}
