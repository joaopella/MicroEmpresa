using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IPerfisFuncionalidadesLogic
    {
        Task<(bool ok, string? error, PerfisFuncionalidadesEntity? item)> GetAsync(int idPerfil, int idFuncao);
        Task<IReadOnlyList<PerfisFuncionalidadesEntity>> ListByPerfilAsync(int idPerfil);
        Task<IReadOnlyList<PerfisFuncionalidadesEntity>> ListByFuncaoAsync(int idFuncao);

        // cria ou atualiza a permissão (upsert)
        Task<(bool ok, string? error, PerfisFuncionalidadesEntity? saved)> SetPermissaoAsync(
            int idPerfil, int idFuncao, string crud);

        Task<(bool ok, string? error)> DeleteAsync(int idPerfil, int idFuncao);
    }
}
