using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IPerfisLogic
    {
        Task<(IReadOnlyList<PerfisEntity> items, int total)> ListAsync(int page, int pageSize, string? search);
        Task<PerfisEntity?> GetAsync(int id);
        Task<ResponseMessage> CreateAsync(PerfisEntity entity);
        Task<(bool ok, string? error, PerfisEntity? updated)> UpdateAsync(int id, PerfisEntity entity);
        Task<(bool ok, string? error)> DeleteAsync(int id);
    }
}
