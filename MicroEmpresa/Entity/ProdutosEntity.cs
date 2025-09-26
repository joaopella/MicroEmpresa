namespace MicroEmpresa.Entity
{
    public class ProdutosEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }

        public string Nome { get; set; } = default!;
        public string? Sku { get; set; }                  // opcional
        public string Tipo { get; set; } = default!;      // "produto" | "servico"
        public string? Unidade { get; set; }              // UN, KG, etc.

        public decimal PrecoVenda { get; set; }           // dbo: preco_venda (18,2)
        public decimal Custo { get; set; }                // dbo: custo (18,2)
        public bool Ativo { get; set; }                   // dbo: ativo (bit)

        public decimal MarkupPercentual { get; set; }     // dbo: markup_percentual (9,2)
        public decimal PrecoSugerido { get; set; }        // dbo: preco_sugerido (18,2)

        // Navegação
        public LojasEntity Loja { get; set; } = default!;
    }
}
