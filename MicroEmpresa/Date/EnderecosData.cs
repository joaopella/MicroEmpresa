using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class EnderecosData : IEnderecosRepository
    {
        private readonly AppDbContext _db;
        public EnderecosData(AppDbContext db) => _db = db;

        public Task<List<EnderecosEntity>> ListarAsync() =>
            _db.Enderecos.AsNoTracking()
                .OrderBy(e => e.Id)
                .ToListAsync();

        public Task<EnderecosEntity?> ObterAsync(int id) =>
            _db.Enderecos.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task CriarAsync(EnderecosEntity entity)
        {
            entity.CriadoEm = DateTime.UtcNow;
            _db.Enderecos.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> AtualizarAsync(int id, EnderecosEntity entity)
        {
            entity.Id = id;
            entity.AtualizadoEm = DateTime.UtcNow;

            _db.Attach(entity);

            if (!string.IsNullOrWhiteSpace(entity.Tipo))
            {
                _db.Entry(entity).Property(x => x.Tipo).IsModified = true;
            }
                
            if (!string.IsNullOrWhiteSpace(entity.Logradouro))
            {
                _db.Entry(entity).Property(x => x.Logradouro).IsModified = true;
            }
                
            if (!string.IsNullOrWhiteSpace(entity.Numero))
            {
                _db.Entry(entity).Property(x => x.Numero).IsModified = true;
            }
                
            if (!string.IsNullOrWhiteSpace(entity.Complemento))
            {
                _db.Entry(entity).Property(x => x.Complemento).IsModified = true;
            }
                
            if (!string.IsNullOrWhiteSpace(entity.Bairro))
            {
                _db.Entry(entity).Property(x => x.Bairro).IsModified = true;
            }
                
            if (!string.IsNullOrWhiteSpace(entity.Cidade))
            {
                _db.Entry(entity).Property(x => x.Cidade).IsModified = true;
            }
                
            if (!string.IsNullOrWhiteSpace(entity.Uf))
            {
                _db.Entry(entity).Property(x => x.Uf).IsModified = true;
            }
                
            if (!string.IsNullOrWhiteSpace(entity.Cep))
            {
                _db.Entry(entity).Property(x => x.Cep).IsModified = true;
            }
                
            if (entity.IdCliente.HasValue)
            {
                _db.Entry(entity).Property(x => x.IdCliente).IsModified = true;
            }
                
            if (entity.IdLoja.HasValue) 
            {
                _db.Entry(entity).Property(x => x.IdLoja).IsModified = true;
            }
               
            _db.Entry(entity).Property(x => x.AtualizadoEm).IsModified = true;

            var linhas = await _db.SaveChangesAsync();
            return linhas > 0;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var atual = await _db.Enderecos.FirstOrDefaultAsync(e => e.Id == id);
            if (atual is null) return false;

            _db.Enderecos.Remove(atual);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
