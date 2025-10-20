using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Logic
{
    public class FuncionalidadesLogic : IFuncionalidadesLogic
    {
        private readonly IFuncionalidadesRepository _repo;
        public FuncionalidadesLogic(IFuncionalidadesRepository repo) => _repo = repo;

        public async Task<(IReadOnlyList<FuncionalidadesEntity> items, int total)> ListAsync(int page, int pageSize, string? search)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;
            var skip = (page - 1) * pageSize;

            var total = await _repo.CountAsync(search);
            var items = await _repo.GetAllAsync(skip, pageSize, search);
            return (items, total);
        }

        public Task<FuncionalidadesEntity?> GetAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<(bool ok, string? error, FuncionalidadesEntity? created)> CreateAsync(FuncionalidadesEntity entity)
        {
            entity.Descricao = entity.Descricao?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(entity.Descricao))
                return (false, "Descrição é obrigatória.", null);

            if (await _repo.ExistsByDescricaoAsync(entity.Descricao))
                return (false, "Já existe uma funcionalidade com essa descrição.", null);

            var created = await _repo.AddAsync(entity);
            return (true, null, created);
        }

        public async Task<(bool ok, string? error, FuncionalidadesEntity? updated)> UpdateAsync(int id, FuncionalidadesEntity entity)
        {
            var current = await _repo.GetByIdAsync(id);
            if (current is null) return (false, "Funcionalidade não encontrada.", null);

            var nova = entity.Descricao?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(nova))
                return (false, "Descrição é obrigatória.", null);

            if (await _repo.ExistsByDescricaoAsync(nova, ignoreId: id))
                return (false, "Já existe uma funcionalidade com essa descrição.", null);

            current.Descricao = nova;

            var updated = await _repo.UpdateAsync(current);
            return (true, null, updated);
        }

        public async Task<(bool ok, string? error)> DeleteAsync(int id)
        {
            try
            {
                var ok = await _repo.DeleteAsync(id);
                return ok ? (true, null) : (false, "Funcionalidade não encontrada.");
            }
            catch (DbUpdateException)
            {
                return (false, "Não é possível excluir: há vínculos em perfis/permissões.");
            }
        }
    }
}
