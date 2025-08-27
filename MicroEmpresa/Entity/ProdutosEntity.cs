namespace MicroEmpresa.Entity
{
    public class ProdutosEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public string Nome { get; set; } = default!;
        public string? Sku { get; set; }
        public string Tipo { get; set; } = default!;   // "produto" | "servico"
        public string? Unidade { get; set; }           // UN, KG, etc.
        public decimal PrecoVenda { get; set; }
        public decimal Custo { get; set; }
        public bool Ativo { get; set; }

        // Navegação
        public LojasEntity Loja { get; set; } = default!;
    }
}
