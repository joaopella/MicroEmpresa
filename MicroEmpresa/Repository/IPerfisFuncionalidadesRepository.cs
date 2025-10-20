using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IPerfisFuncionalidadesRepository
    {
        Task<PerfisFuncionalidadesEntity?> GetAsync(int idPerfil, int idFuncao);
        Task<IReadOnlyList<PerfisFuncionalidadesEntity>> ListByPerfilAsync(int idPerfil);
        Task<IReadOnlyList<PerfisFuncionalidadesEntity>> ListByFuncaoAsync(int idFuncao);

        Task<PerfisFuncionalidadesEntity> AddAsync(PerfisFuncionalidadesEntity entity);
        Task<PerfisFuncionalidadesEntity> UpdateAsync(PerfisFuncionalidadesEntity entity);
        Task<bool> DeleteAsync(int idPerfil, int idFuncao);

        Task<bool> PerfilExisteAsync(int idPerfil);
        Task<bool> FuncaoExisteAsync(int idFuncao);
    }
}
