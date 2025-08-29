namespace MicroEmpresa.Entity
{
    public class MovEstoqueEntity
    {
        public int Id { get; set; }
        public int IdEstoque { get; set; }

        public string Tipo { get; set; } = default!;         // ENTRADA | SAIDA
        public decimal Quantidade { get; set; }
        public DateTime DataMov { get; set; }
        public string? Obs { get; set; }

        public byte[] Rv { get; set; } = default!;

        // Navegação
        public EstoquesEntity Estoque { get; set; } = default!;
    }
}
