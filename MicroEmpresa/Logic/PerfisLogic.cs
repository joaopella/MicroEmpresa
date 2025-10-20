using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Logic
{
    public class PerfisLogic : IPerfisLogic
    {
        private readonly IPerfisRepository _repo;
        public PerfisLogic(IPerfisRepository repo) => _repo = repo;

        public async Task<(IReadOnlyList<PerfisEntity> items, int total)> ListAsync(int page, int pageSize, string? search)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;
            var skip = (page - 1) * pageSize;

            var total = await _repo.CountAsync(search);
            var items = await _repo.GetAllAsync(skip, pageSize, search);
            return (items, total);
        }

        public Task<PerfisEntity?> GetAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<ResponseMessage>CreateAsync(PerfisEntity entity)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.Nome.Trim()))
                {
                    return new ResponseMessage { Message = "Nome é obrigatório." };
                }
                if (string.IsNullOrEmpty(entity.Descricao.Trim()))
                {
                    return new ResponseMessage { Message = "A Descrição é obrigatório." };
                }

                var created = await _repo.AddAsync(entity);

                return new ResponseMessage
                {
                    Message = "Loja cadastrada com sucesso!",
                };
            }
            catch (Exception ex)
            {
                throw ex;            
            }
        }

        public async Task<(bool ok, string? error, PerfisEntity? updated)> UpdateAsync(int id, PerfisEntity entity)
        {
            var current = await _repo.GetByIdAsync(id);
            if (current is null) return (false, "Perfil não encontrado.", null);

            // Concorrência: exige Rv igual ao do banco
            if (entity.Rv == null || entity.Rv.Length == 0 || !current.Rv.SequenceEqual(entity.Rv))
                return (false, "Registro alterado por outro usuário. Recarregue e tente novamente.", null);

            var novoNome = entity.Nome?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(novoNome))
                return (false, "Nome é obrigatório.", null);

            if (await _repo.ExistsByNameAsync(novoNome, ignoreId: id))
                return (false, "Já existe um perfil com esse nome.", null);

            current.Nome = novoNome;
            current.Descricao = string.IsNullOrWhiteSpace(entity.Descricao) ? null : entity.Descricao!.Trim();

            try
            {
                var updated = await _repo.UpdateAsync(current);
                return (true, null, updated);
            }
            catch (DbUpdateConcurrencyException)
            {
                return (false, "Falha de concorrência ao atualizar.", null);
            }
        }

        public async Task<(bool ok, string? error)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok ? (true, null) : (false, "Perfil não encontrado.");
        }
    }
}
