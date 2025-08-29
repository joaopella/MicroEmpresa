namespace MicroEmpresa.Entity
{
    public class EstoquesEntity
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public int IdProduto { get; set; }

        public decimal Saldo { get; set; }

        public byte[] Rv { get; set; } = default!;

        // Navegações
        public LojasEntity Loja { get; set; } = default!;
        public ProdutosEntity Produto { get; set; } = default!;
        public ICollection<MovEstoqueEntity> Movimentos { get; set; } = new List<MovEstoqueEntity>();
    }
}
