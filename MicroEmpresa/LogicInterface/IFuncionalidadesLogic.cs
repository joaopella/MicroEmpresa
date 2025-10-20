using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IFuncionalidadesLogic
    {
        Task<(IReadOnlyList<FuncionalidadesEntity> items, int total)> ListAsync(int page, int pageSize, string? search);
        Task<FuncionalidadesEntity?> GetAsync(int id);
        Task<(bool ok, string? error, FuncionalidadesEntity? created)> CreateAsync(FuncionalidadesEntity entity);
        Task<(bool ok, string? error, FuncionalidadesEntity? updated)> UpdateAsync(int id, FuncionalidadesEntity entity);
        Task<(bool ok, string? error)> DeleteAsync(int id);
    }
}
