using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class ProdutosData : IProdutosRepository
    {
        private readonly AppDbContext _ctx;
        public ProdutosData(AppDbContext ctx) => _ctx = ctx;

        public Task<List<ProdutosEntity>> ListarAsync() =>
            _ctx.Set<ProdutosEntity>()
                .AsNoTracking()
                .Include(x => x.Loja)
                .ToListAsync();

        public Task<List<ProdutosEntity>> ListarPorLojaAsync(int idLoja) =>
            _ctx.Set<ProdutosEntity>()
                .AsNoTracking()
                .Where(x => x.IdLoja == idLoja)
                .Include(x => x.Loja)
                .ToListAsync();

        public Task<ProdutosEntity?> ObterAsync(int id) =>
            _ctx.Set<ProdutosEntity>()
                .Include(x => x.Loja)
                .FirstOrDefaultAsync(x => x.Id == id);

        public Task<ProdutosEntity?> ObterPorLojaSkuAsync(int idLoja, string sku) =>
            _ctx.Set<ProdutosEntity>()
                .Include(x => x.Loja)
                .FirstOrDefaultAsync(x => x.IdLoja == idLoja && x.Sku == sku);

        public async Task CriarAsync(ProdutosEntity entity)
        {
            _ctx.Set<ProdutosEntity>().Add(entity);
            await _ctx.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza todos os campos. Requer Rv para concorrência.
        /// </summary>
        public async Task<bool> AtualizarAsync(ProdutosEntity e)
        {
            var tracked = await _ctx.Set<ProdutosEntity>().FirstOrDefaultAsync(x => x.Id == e.Id);
            if (tracked is null) return false;

            _ctx.Entry(tracked).Property(p => p.Rv).OriginalValue = e.Rv;

            tracked.IdLoja = e.IdLoja;
            tracked.Nome = e.Nome;
            tracked.Sku = e.Sku;
            tracked.Tipo = e.Tipo;
            tracked.Unidade = e.Unidade;
            tracked.PrecoVenda = e.PrecoVenda;
            tracked.Custo = e.Custo;
            tracked.Ativo = e.Ativo;
            tracked.MarkupPercentual = e.MarkupPercentual;
            tracked.PrecoSugerido = e.PrecoSugerido;
            tracked.AtualizadoEm = DateTime.UtcNow;

            await _ctx.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Atualização parcial; se setLojaSku=false, mantém IdLoja e Sku originais.
        /// </summary>
        public async Task<bool> AtualizarCamposAsync(ProdutosEntity e, bool setLojaSku)
        {
            var tracked = await _ctx.Set<ProdutosEntity>().FirstOrDefaultAsync(x => x.Id == e.Id);
            if (tracked is null) return false;

            _ctx.Entry(tracked).Property(p => p.Rv).OriginalValue = e.Rv;

            if (setLojaSku)
            {
                tracked.IdLoja = e.IdLoja;
                tracked.Sku = e.Sku;
            }

            // Mapeia apenas se veio valor (strings não nulas, decimais preenchidos etc.)
            if (!string.IsNullOrWhiteSpace(e.Nome)) tracked.Nome = e.Nome;
            if (!string.IsNullOrWhiteSpace(e.Tipo)) tracked.Tipo = e.Tipo;
            if (!string.IsNullOrWhiteSpace(e.Unidade)) tracked.Unidade = e.Unidade;

            if (e.PrecoVenda >= 0) tracked.PrecoVenda = e.PrecoVenda;
            if (e.Custo >= 0) tracked.Custo = e.Custo;
            if (e.MarkupPercentual >= 0) tracked.MarkupPercentual = e.MarkupPercentual;
            if (e.PrecoSugerido >= 0) tracked.PrecoSugerido = e.PrecoSugerido;

            // bool: consideramos o valor vindo (default false = pode confundir). Ideal usar DTO para parcial.
            tracked.Ativo = e.Ativo;

            tracked.AtualizadoEm = DateTime.UtcNow;

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var e = await _ctx.Set<ProdutosEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (e is null) return false;

            _ctx.Remove(e);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
