using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class LojasData : ILojasRepository
    {
        private readonly AppDbContext _db;
        public LojasData(AppDbContext db) => _db = db;

        public Task<List<LojasEntity>> ListarAsync() =>
            _db.Lojas.AsNoTracking().OrderBy(l => l.NomeFantasia).ToListAsync();

        public Task<LojasEntity?> ObterAsync(int id) =>
            _db.Lojas.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);

        public Task<bool> CnpjExisteAsync(string cnpj)
        {
            return _db.Lojas.AsNoTracking().AnyAsync(l => l.Cnpj == cnpj);
        }

        public Task<bool> TemDependenciasAsync(int id) =>
            Task.FromResult(
                _db.Produtos.Any(p => p.IdLoja == id) ||
                _db.Funcionarios.Any(f => f.IdLoja == id) ||
                _db.Clientes.Any(c => c.IdLoja == id));

        public async Task<LojasEntity> CriarAsync(LojasEntity entity)
        {
            try
            {
                entity.CriadoEm = DateTime.UtcNow;
                _db.Lojas.Add(entity);
                await _db.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Erro ao atualizar a loja.", ex);
            }

        }

        public async Task<bool> AtualizarAsync(int id, LojasEntity entity)
        {
            try
            {
                entity.Id = id;
                entity.AtualizadoEm = DateTime.UtcNow;

                _db.Attach(entity);

                if (!string.IsNullOrWhiteSpace(entity.NomeFantasia))
                {
                    _db.Entry(entity).Property(x => x.NomeFantasia).IsModified = true;
                }

                if (!string.IsNullOrWhiteSpace(entity.Cnpj))
                {
                    _db.Entry(entity).Property(x => x.Cnpj).IsModified = true;
                }

                if (!string.IsNullOrWhiteSpace(entity.Telefone))
                {
                    _db.Entry(entity).Property(x => x.Telefone).IsModified = true;
                }

                _db.Entry(entity).Property(x => x.AtualizadoEm).IsModified = true;

                var linhas = await _db.SaveChangesAsync();
                return linhas > 0;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Erro ao atualizar a loja.", ex);
            }
        }


        public async Task<bool> RemoverAsync(int id)
        {
            var atual = await _db.Lojas.FirstOrDefaultAsync(l => l.Id == id);
            if (atual is null) return false;

            _db.Lojas.Remove(atual);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}


