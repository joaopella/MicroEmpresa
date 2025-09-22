using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class MovEstoqueData : IMovEstoqueRepository
    {
        private readonly AppDbContext _ctx;
        public MovEstoqueData(AppDbContext ctx) => _ctx = ctx;

        public Task<List<MovEstoqueEntity>> ListarPorLojaProdutoAsync(int idLoja, int idProduto) =>
            _ctx.Set<MovEstoqueEntity>()
                .AsNoTracking()
                .Where(m => m.IdLoja == idLoja && m.IdProduto == idProduto)
                .OrderByDescending(m => m.DataMov)
                .ToListAsync();

        public Task<MovEstoqueEntity?> ObterAsync(int id) =>
            _ctx.Set<MovEstoqueEntity>().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<int> CriarComAjusteAsync(MovEstoqueEntity mov)
        {
            // transação para garantir consistência entre movimento e estoque
            using var tx = await _ctx.Database.BeginTransactionAsync();

            // obtém estoque; se não existir, cria com saldo 0
            var est = await _ctx.Set<EstoquesEntity>()
                                .FirstOrDefaultAsync(e => e.IdLoja == mov.IdLoja && e.IdProduto == mov.IdProduto);

            if (est is null)
            {
                est = new EstoquesEntity
                {
                    IdLoja = mov.IdLoja,
                    IdProduto = mov.IdProduto,
                    Saldo = 0m
                };
                _ctx.Set<EstoquesEntity>().Add(est);
                await _ctx.SaveChangesAsync();
            }

            // calcula delta
            var isEntrada = string.Equals(mov.Tipo, "entrada", StringComparison.OrdinalIgnoreCase);
            var isSaida = string.Equals(mov.Tipo, "saida", StringComparison.OrdinalIgnoreCase);

            if (!isEntrada && !isSaida)
                throw new InvalidOperationException("Tipo inválido. Use 'entrada' ou 'saida'.");

            var delta = isEntrada ? mov.Qtd : -mov.Qtd;

            // valida saldo
            var novoSaldo = est.Saldo + delta;
            if (novoSaldo < 0m)
                throw new InvalidOperationException("Saldo insuficiente para esta saída de estoque.");

            // aplica
            est.Saldo = novoSaldo;

            // insere movimento
            if (mov.DataMov == default) mov.DataMov = DateTime.UtcNow;
            _ctx.Set<MovEstoqueEntity>().Add(mov);

            await _ctx.SaveChangesAsync();
            await tx.CommitAsync();

            return mov.Id;
        }

        public async Task<bool> ExcluirAsync(int id, bool estornar)
        {
            using var tx = await _ctx.Database.BeginTransactionAsync();

            var mov = await _ctx.Set<MovEstoqueEntity>().FirstOrDefaultAsync(m => m.Id == id);
            if (mov is null) return false;

            if (estornar)
            {
                var est = await _ctx.Set<EstoquesEntity>()
                                    .FirstOrDefaultAsync(e => e.IdLoja == mov.IdLoja && e.IdProduto == mov.IdProduto);
                if (est is null) return false;

                var isEntrada = string.Equals(mov.Tipo, "entrada", StringComparison.OrdinalIgnoreCase);
                var deltaEstorno = isEntrada ? -mov.Qtd : mov.Qtd; // desfaz o efeito

                var novoSaldo = est.Saldo + deltaEstorno;
                if (novoSaldo < 0m)
                    throw new InvalidOperationException("Estorno geraria saldo negativo.");

                est.Saldo = novoSaldo;
            }

            _ctx.Remove(mov);
            await _ctx.SaveChangesAsync();
            await tx.CommitAsync();

            return true;
        }
    }
}
