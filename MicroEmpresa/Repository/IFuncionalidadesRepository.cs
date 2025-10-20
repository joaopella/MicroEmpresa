using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IFuncionalidadesRepository
    {
        Task<FuncionalidadesEntity?> GetByIdAsync(int id);
        Task<IReadOnlyList<FuncionalidadesEntity>> GetAllAsync(int skip, int take, string? search);
        Task<int> CountAsync(string? search);
        Task<bool> ExistsByDescricaoAsync(string descricao, int? ignoreId = null);
        Task<FuncionalidadesEntity> AddAsync(FuncionalidadesEntity entity);
        Task<FuncionalidadesEntity> UpdateAsync(FuncionalidadesEntity entity);
        Task<bool> DeleteAsync(int id);
    }
}
