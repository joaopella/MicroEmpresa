using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class MovCaixaData : IMovCaixaRepository
    {
        private readonly AppDbContext _ctx;
        public MovCaixaData(AppDbContext ctx) => _ctx = ctx;

        public Task<List<MovCaixaEntity>> ListarPorCaixaAsync(int idCaixa) =>
            _ctx.Set<MovCaixaEntity>()
                .AsNoTracking()
                .Include(x => x.Pagamento)
                .Where(x => x.IdCaixa == idCaixa)
                .OrderByDescending(x => x.DataMov)
                .ToListAsync();

        public Task<MovCaixaEntity?> ObterAsync(int id) =>
            _ctx.Set<MovCaixaEntity>()
                .Include(x => x.Caixa)
                .Include(x => x.Pagamento)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<int> CriarAsync(MovCaixaEntity e)
        {
            e.CriadoEm = DateTime.UtcNow;
            e.AtualizadoEm = DateTime.UtcNow;

            _ctx.Set<MovCaixaEntity>().Add(e);
            await _ctx.SaveChangesAsync();
            return e.Id;
        }

        public async Task<bool> AtualizarDescricaoAsync(int id, byte[] rv, string? descricao)
        {
            var m = await _ctx.Set<MovCaixaEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (m is null) return false;

            _ctx.Entry(m).Property(p => p.Rv).OriginalValue = rv;
            m.Descricao = descricao;
            m.AtualizadoEm = DateTime.UtcNow;

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var m = await _ctx.Set<MovCaixaEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (m is null) return false;

            _ctx.Remove(m);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
