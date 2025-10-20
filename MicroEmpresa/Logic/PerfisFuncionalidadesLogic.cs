using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;

namespace MicroEmpresa.Logic
{
    public class PerfisFuncionalidadesLogic : IPerfisFuncionalidadesLogic
    {
        private readonly IPerfisFuncionalidadesRepository _repo;
        public PerfisFuncionalidadesLogic(IPerfisFuncionalidadesRepository repo) => _repo = repo;

        public async Task<(bool ok, string? error, PerfisFuncionalidadesEntity? item)> GetAsync(int idPerfil, int idFuncao)
        {
            var item = await _repo.GetAsync(idPerfil, idFuncao);
            return item is null ? (false, "Permissão não encontrada.", null) : (true, null, item);
        }

        public Task<IReadOnlyList<PerfisFuncionalidadesEntity>> ListByPerfilAsync(int idPerfil) =>
            _repo.ListByPerfilAsync(idPerfil);

        public Task<IReadOnlyList<PerfisFuncionalidadesEntity>> ListByFuncaoAsync(int idFuncao) =>
            _repo.ListByFuncaoAsync(idFuncao);

        public async Task<(bool ok, string? error, PerfisFuncionalidadesEntity? saved)> SetPermissaoAsync(
            int idPerfil, int idFuncao, string crud)
        {
            // validações básicas
            if (!await _repo.PerfilExisteAsync(idPerfil))
                return (false, "Perfil inválido.", null);

            if (!await _repo.FuncaoExisteAsync(idFuncao))
                return (false, "Funcionalidade inválida.", null);

            crud = (crud ?? "").Trim().ToUpperInvariant();
            if (crud.Length != 4 || crud.Any(c => c != 'X' && c != '0'))
                return (false, "CRUD inválido. Use 4 caracteres com 'X' ou '0' (ordem C R U D).", null);

            var atual = await _repo.GetAsync(idPerfil, idFuncao);
            if (atual is null)
            {
                var novo = new PerfisFuncionalidadesEntity
                {
                    IdPerfil = idPerfil,
                    IdFuncao = idFuncao,
                    Crud = crud,
                    CriadoEm = DateTime.UtcNow,
                    AtualizadoEm = DateTime.UtcNow
                };
                var saved = await _repo.AddAsync(novo);
                return (true, null, saved);
            }
            else
            {
                atual.Crud = crud;
                atual.AtualizadoEm = DateTime.UtcNow;
                var saved = await _repo.UpdateAsync(atual);
                return (true, null, saved);
            }
        }

        public async Task<(bool ok, string? error)> DeleteAsync(int idPerfil, int idFuncao)
        {
            var ok = await _repo.DeleteAsync(idPerfil, idFuncao);
            return ok ? (true, null) : (false, "Permissão não encontrada.");
        }
    }
}
