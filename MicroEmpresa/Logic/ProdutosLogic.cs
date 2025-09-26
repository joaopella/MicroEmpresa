using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Logic
{
    public class ProdutosLogic : IProdutosLogic
    {
        private readonly IProdutosRepository _repo;
        public ProdutosLogic(IProdutosRepository repo) => _repo = repo;

        public Task<List<ProdutosEntity>> ListarAsync() => _repo.ListarAsync();
        public Task<List<ProdutosEntity>> ListarPorLojaAsync(int idLoja) => _repo.ListarPorLojaAsync(idLoja);
        public Task<ProdutosEntity?> ObterAsync(int id) => _repo.ObterAsync(id);
        public Task<ProdutosEntity?> ObterPorLojaSkuAsync(int idLoja, string sku) => _repo.ObterPorLojaSkuAsync(idLoja, sku);

        public async Task<ResponseMessage> CriarAsync(ProdutosEntity e)
        {
            // validações básicas
            if (e.IdLoja <= 0) return new ResponseMessage { Message = "IdLoja inválido." };
            if (string.IsNullOrWhiteSpace(e.Nome)) return new ResponseMessage { Message = "Nome é obrigatório." };
            if (string.IsNullOrWhiteSpace(e.Sku)) return new ResponseMessage { Message = "SKU é obrigatório." };
            if (e.PrecoVenda < 0) return new ResponseMessage { Message = "Preço de venda não pode ser negativo." };
            if (e.Custo < 0) return new ResponseMessage { Message = "Custo não pode ser negativo." };
            if (e.MarkupPercentual < 0) e.MarkupPercentual = 0;

            // regra: SKU único por loja
            var dup = await _repo.ObterPorLojaSkuAsync(e.IdLoja, e.Sku);
            if (dup is not null) return new ResponseMessage { Message = "Já existe produto com esse SKU nesta loja." };

            // opcional: calcular preco_sugerido se não vier definido (custo * (1 + markup%/100))
            if (e.PrecoSugerido < 0 || e.PrecoSugerido == 0)
            {
                if (e.Custo >= 0 && e.MarkupPercentual >= 0)
                    e.PrecoSugerido = Math.Round(e.Custo * (1 + (e.MarkupPercentual / 100m)), 2);
            }

            e.CriadoEm = DateTime.UtcNow;
            e.AtualizadoEm = DateTime.UtcNow;

            await _repo.CriarAsync(e);
            return new ResponseMessage { Message = "OK" };
        }

        public async Task<ResponseMessage> AtualizarAsync(ProdutosEntity e)
        {
            if (e.Id <= 0) return new ResponseMessage { Message = "ID inválido." };
            if (e.IdLoja <= 0) return new ResponseMessage { Message = "IdLoja inválido." };
            if (string.IsNullOrWhiteSpace(e.Nome)) return new ResponseMessage { Message = "Nome é obrigatório." };
            if (string.IsNullOrWhiteSpace(e.Sku)) return new ResponseMessage { Message = "SKU é obrigatório." };
            if (e.PrecoVenda < 0) return new ResponseMessage { Message = "Preço de venda não pode ser negativo." };
            if (e.Custo < 0) return new ResponseMessage { Message = "Custo não pode ser negativo." };
            if (e.Rv is null || e.Rv.Length == 0) return new ResponseMessage { Message = "RowVersion (Rv) é obrigatório." };

            // se mudou (Loja, SKU), verifica duplicidade
            var existente = await _repo.ObterPorLojaSkuAsync(e.IdLoja, e.Sku);
            if (existente is not null && existente.Id != e.Id)
                return new ResponseMessage { Message = "Já existe produto com esse SKU nesta loja." };

            try
            {
                var ok = await _repo.AtualizarAsync(e);
                return ok ? new ResponseMessage { Message = "OK" }
                          : new ResponseMessage { Message = "Produto não encontrado." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Concorrência detectada. O registro foi alterado por outro usuário." };
            }
        }

        public async Task<ResponseMessage> AtualizarPrecosAsync(int id, decimal? precoVenda, decimal? custo, decimal? markupPercentual, byte[] rv)
        {
            if (id <= 0) return new ResponseMessage { Message = "ID inválido." };
            if (rv is null || rv.Length == 0) return new ResponseMessage { Message = "RowVersion (Rv) é obrigatório." };

            var prod = await _repo.ObterAsync(id);
            if (prod is null) return new ResponseMessage { Message = "Produto não encontrado." };

            if (precoVenda is < 0 || custo is < 0 || markupPercentual is < 0)
                return new ResponseMessage { Message = "Valores não podem ser negativos." };

            // aplica somente o que veio
            if (precoVenda is not null) prod.PrecoVenda = precoVenda.Value;
            if (custo is not null) prod.Custo = custo.Value;
            if (markupPercentual is not null) prod.MarkupPercentual = markupPercentual.Value;

            // recalcula sugerido, se tiver custo/markup válidos (opcional)
            if (prod.Custo >= 0 && prod.MarkupPercentual >= 0)
                prod.PrecoSugerido = Math.Round(prod.Custo * (1 + (prod.MarkupPercentual / 100m)), 2);

            prod.Rv = rv;

            try
            {
                var ok = await _repo.AtualizarCamposAsync(prod, setLojaSku: false);
                return ok ? new ResponseMessage { Message = "OK" }
                          : new ResponseMessage { Message = "Produto não encontrado." };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ResponseMessage { Message = "Concorrência detectada. Atualize e tente novamente." };
            }
        }

        public async Task<ResponseMessage> ExcluirAsync(int id)
        {
            var ok = await _repo.ExcluirAsync(id);
            return ok ? new ResponseMessage { Message = "OK" }
                      : new ResponseMessage { Message = "Produto não encontrado." };
        }
    }
}
