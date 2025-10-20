using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IPerfisRepository
    {
        Task<PerfisEntity?> GetByIdAsync(int id);
        Task<IReadOnlyList<PerfisEntity>> GetAllAsync(int skip, int take, string? search);
        Task<int> CountAsync(string? search);
        Task<PerfisEntity> AddAsync(PerfisEntity entity);
        Task<PerfisEntity> UpdateAsync(PerfisEntity entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByNameAsync(string nome, int? ignoreId = null);
    }
}
