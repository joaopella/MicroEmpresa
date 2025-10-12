using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class VendasData : IVendasRepository
    {
        private readonly AppDbContext _ctx;
        public VendasData(AppDbContext ctx) => _ctx = ctx;

        public Task<List<VendasEntity>> ListarAsync() =>
            _ctx.Vendas.AsNoTracking().ToListAsync();

        public Task<VendasEntity?> ObterAsync(int id) =>
            _ctx.Vendas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<int> CriarAsync(VendasEntity venda)
        {
            await _ctx.Vendas.AddAsync(venda);
            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> AtualizarAsync(VendasEntity venda)
        {
            _ctx.Vendas.Attach(venda);
            _ctx.Entry(venda).Property(x => x.Rv).OriginalValue = venda.Rv; // controle de concorrência
            _ctx.Entry(venda).State = EntityState.Modified;
            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> RemoverAsync(int id, byte[] rv)
        {
            var ent = new VendasEntity { Id = id, Rv = rv };
            _ctx.Vendas.Attach(ent);
            _ctx.Entry(ent).Property(x => x.Rv).OriginalValue = rv;
            _ctx.Vendas.Remove(ent);
            return await _ctx.SaveChangesAsync();
        }
    }
}
