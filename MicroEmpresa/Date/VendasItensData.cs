using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class VendasItensData : IVendasItensRepository
    {
        private readonly AppDbContext _ctx;
        public VendasItensData(AppDbContext ctx) => _ctx = ctx;

        public Task<List<VendasItensEntity>> ListarAsync() =>
            _ctx.VendasItens.AsNoTracking().ToListAsync();

        public Task<List<VendasItensEntity>> ListarPorVendaAsync(int idVenda) =>
            _ctx.VendasItens.AsNoTracking().Where(x => x.IdVenda == idVenda).ToListAsync();

        public Task<VendasItensEntity?> ObterAsync(int id) =>
            _ctx.VendasItens.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<int> CriarAsync(VendasItensEntity item)
        {
            await _ctx.VendasItens.AddAsync(item);
            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> AtualizarAsync(VendasItensEntity item)
        {
            _ctx.VendasItens.Attach(item);
            _ctx.Entry(item).Property(x => x.Rv).OriginalValue = item.Rv;
            _ctx.Entry(item).State = EntityState.Modified;
            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> RemoverAsync(int id, byte[] rv)
        {
            var ent = new VendasItensEntity { Id = id, Rv = rv };
            _ctx.VendasItens.Attach(ent);
            _ctx.Entry(ent).Property(x => x.Rv).OriginalValue = rv;
            _ctx.VendasItens.Remove(ent);
            return await _ctx.SaveChangesAsync();
        }
    }
}