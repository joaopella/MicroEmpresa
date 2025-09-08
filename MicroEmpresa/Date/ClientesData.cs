using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class ClientesRepository : IClientesRepository
    {
        private readonly AppDbContext _db;
        public ClientesRepository(AppDbContext db) => _db = db;

        public Task<List<ClientesEntity>> ListarAsync() =>
            _db.Clientes.AsNoTracking()
                .OrderBy(c => c.Nome)
                .ToListAsync();

        public Task<ClientesEntity?> ObterAsync(int id) =>
            _db.Clientes.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task CriarAsync(ClientesEntity entity)
        {
            entity.CriadoEm = DateTime.UtcNow;
            _db.Clientes.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> AtualizarAsync(int id, ClientesEntity entity)
        {
            entity.Id = id;
            entity.AtualizadoEm = DateTime.UtcNow;

            _db.Attach(entity);

            if (!string.IsNullOrWhiteSpace(entity.Nome))
                _db.Entry(entity).Property(x => x.Nome).IsModified = true;

            if (!string.IsNullOrWhiteSpace(entity.Cpf))
                _db.Entry(entity).Property(x => x.Cpf).IsModified = true;

            if (!string.IsNullOrWhiteSpace(entity.Email))
                _db.Entry(entity).Property(x => x.Email).IsModified = true;

            if (!string.IsNullOrWhiteSpace(entity.Telefone))
                _db.Entry(entity).Property(x => x.Telefone).IsModified = true;

            _db.Entry(entity).Property(x => x.AtualizadoEm).IsModified = true;

            var linhas = await _db.SaveChangesAsync();
            return linhas > 0;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var atual = await _db.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (atual is null) return false;

            _db.Clientes.Remove(atual);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
