namespace MicroEmpresa.Entity
{
    public class MovEstoqueEntity
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public int IdProduto { get; set; }

        /// <summary>Entrada/Saida (use "entrada" ou "saida")</summary>
        public string Tipo { get; set; } = default!;
        public decimal Qtd { get; set; }          // quantidade movimentada
        public decimal? CustoUnit { get; set; }   // opcional: custo por unidade, quando aplicável
        public string? Motivo { get; set; }       // ex.: compra, venda, ajuste, perda, inventário
        public DateTime DataMov { get; set; }

        public byte[] Rv { get; set; } = default!;

        // Navegações (diretas com loja/produto)
        public LojasEntity Loja { get; set; } = default!;
        public ProdutosEntity Produto { get; set; } = default!;
    }
}
