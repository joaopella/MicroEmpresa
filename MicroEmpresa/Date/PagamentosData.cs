using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class PagamentosData : IPagamentosRepository
    {
        private readonly AppDbContext _ctx;
        public PagamentosData(AppDbContext ctx) => _ctx = ctx;

        public Task<List<PagamentosEntity>> ListarPorVendaAsync(int idVenda) =>
            _ctx.Set<PagamentosEntity>()
                .AsNoTracking()
                .Where(p => p.IdVenda == idVenda)
                .OrderBy(p => p.Id)
                .ToListAsync();

        public Task<PagamentosEntity?> ObterAsync(int id) =>
            _ctx.Set<PagamentosEntity>()
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<int> CriarAsync(PagamentosEntity e)
        {
            e.CriadoEm = DateTime.UtcNow;
            e.AtualizadoEm = DateTime.UtcNow;

            _ctx.Set<PagamentosEntity>().Add(e);
            await _ctx.SaveChangesAsync();
            return e.Id;
        }

        public async Task<bool> AtualizarAsync(PagamentosEntity e)
        {
            var tracked = await _ctx.Set<PagamentosEntity>().FirstOrDefaultAsync(p => p.Id == e.Id);
            if (tracked is null) return false;

            _ctx.Entry(tracked).Property(p => p.Rv).OriginalValue = e.Rv;

            tracked.FormaPagamento = e.FormaPagamento;
            tracked.Valor = e.Valor;
            tracked.Nsu = e.Nsu;
            tracked.Autorizacao = e.Autorizacao;
            tracked.Bandeira = e.Bandeira;
            tracked.AtualizadoEm = DateTime.UtcNow;

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var p = await _ctx.Set<PagamentosEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (p is null) return false;

            _ctx.Remove(p);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
