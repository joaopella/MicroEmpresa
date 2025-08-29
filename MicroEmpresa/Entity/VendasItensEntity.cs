namespace MicroEmpresa.Entity
{
    public class VendasItensEntity
    {
        public int Id { get; set; }
        public int IdVenda { get; set; }
        public int IdProduto { get; set; }

        public decimal Quantidade { get; set; }
        public decimal PrecoUnit { get; set; }
        public decimal? DescontoItem { get; set; }
        public decimal? AcrescimoItem { get; set; }

        public byte[] Rv { get; set; } = default!;

        // Navegações
        public VendasEntity Venda { get; set; } = default!;
        public ProdutosEntity Produto { get; set; } = default!;
    }
}
