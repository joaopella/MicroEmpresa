using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class EstoquesData : IEstoquesRepository
    {
        private readonly AppDbContext _ctx;

        public EstoquesData(AppDbContext ctx) => _ctx = ctx;

        public Task<List<EstoquesEntity>> ListarAsync() =>
            _ctx.Set<EstoquesEntity>()
                .AsNoTracking()
                .Include(x => x.Loja)
                .Include(x => x.Produto)
                .ToListAsync();

        public Task<EstoquesEntity?> ObterAsync(int id) =>
            _ctx.Set<EstoquesEntity>()
                .Include(x => x.Loja)
                .Include(x => x.Produto)
                .FirstOrDefaultAsync(x => x.Id == id);

        public Task<EstoquesEntity?> ObterPorLojaProdutoAsync(int idLoja, int idProduto) =>
            _ctx.Set<EstoquesEntity>()
                .Include(x => x.Loja)
                .Include(x => x.Produto)
                .FirstOrDefaultAsync(x => x.IdLoja == idLoja && x.IdProduto == idProduto);

        public async Task CriarAsync(EstoquesEntity entity)
        {
            _ctx.Set<EstoquesEntity>().Add(entity);
            await _ctx.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza todos os campos (inclui troca de Loja/Produto). Requer Rv para concorrência.
        /// Lança DbUpdateConcurrencyException se o Rv não casar.
        /// </summary>
        public async Task<bool> AtualizarAsync(EstoquesEntity entity)
        {
            var tracked = await _ctx.Set<EstoquesEntity>().FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (tracked is null) return false;

            // aplica o Rv para checagem de concorrência
            _ctx.Entry(tracked).Property(p => p.Rv).OriginalValue = entity.Rv;

            tracked.IdLoja = entity.IdLoja;
            tracked.IdProduto = entity.IdProduto;
            tracked.Saldo = entity.Saldo;

            await _ctx.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Atualiza apenas saldo (ou campos simples), preservando Loja/Produto se setLojaProduto=false.
        /// Também respeita rowversion.
        /// </summary>
        public async Task<bool> AtualizarCamposAsync(EstoquesEntity entity, bool setLojaProduto)
        {
            var tracked = await _ctx.Set<EstoquesEntity>().FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (tracked is null) return false;

            _ctx.Entry(tracked).Property(p => p.Rv).OriginalValue = entity.Rv;

            if (setLojaProduto)
            {
                tracked.IdLoja = entity.IdLoja;
                tracked.IdProduto = entity.IdProduto;
            }

            tracked.Saldo = entity.Saldo;

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var e = await _ctx.Set<EstoquesEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (e is null) return false;

            _ctx.Remove(e);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
