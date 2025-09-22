using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class CaixasData : ICaixasRepository
    {
        private readonly AppDbContext _ctx;
        public CaixasData(AppDbContext ctx) => _ctx = ctx;

        public Task<List<CaixasEntity>> ListarAsync() =>
            _ctx.Set<CaixasEntity>()
                .AsNoTracking()
                .Include(x => x.Loja)
                .Include(x => x.FuncionarioAbertura)
                .Include(x => x.FuncionarioFechamento)
                .OrderByDescending(x => x.DataAbertura)
                .ToListAsync();

        public Task<CaixasEntity?> ObterAsync(int id) =>
            _ctx.Set<CaixasEntity>()
                .Include(x => x.Loja)
                .Include(x => x.FuncionarioAbertura)
                .Include(x => x.FuncionarioFechamento)
                .FirstOrDefaultAsync(x => x.Id == id);

        public Task<CaixasEntity?> ObterAbertoPorLojaAsync(int idLoja) =>
            _ctx.Set<CaixasEntity>()
                .Include(x => x.FuncionarioAbertura)
                .Where(x => x.IdLoja == idLoja && x.DataFechamento == null)
                .OrderByDescending(x => x.DataAbertura)
                .FirstOrDefaultAsync();

        public async Task<int> AbrirAsync(CaixasEntity e)
        {
            _ctx.Set<CaixasEntity>().Add(e);
            await _ctx.SaveChangesAsync();
            return e.Id;
        }

        public async Task<bool> FecharAsync(int id, byte[] rv, int idFunc, decimal valorFechamento, DateTime dataFechamento)
        {
            var c = await _ctx.Set<CaixasEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (c is null) return false;

            // concorrência
            _ctx.Entry(c).Property(p => p.Rv).OriginalValue = rv;

            c.IdFuncionarioFechamento = idFunc;
            c.ValorFechamento = valorFechamento;
            c.DataFechamento = dataFechamento;

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AtualizarObsAsync(int id, byte[] rv, string? obs)
        {
            var c = await _ctx.Set<CaixasEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (c is null) return false;

            _ctx.Entry(c).Property(p => p.Rv).OriginalValue = rv;
            c.Obs = obs;

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var c = await _ctx.Set<CaixasEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (c is null) return false;

            _ctx.Remove(c);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
