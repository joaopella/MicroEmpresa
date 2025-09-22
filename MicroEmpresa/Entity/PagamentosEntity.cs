namespace MicroEmpresa.Entity
{
    public class PagamentosEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int IdVenda { get; set; }

        public string FormaPagamento { get; set; } = default!; // DINHEIRO | PIX | CREDITO | DEBITO...
        public decimal Valor { get; set; }

        public string? Nsu { get; set; }
        public string? Autorizacao { get; set; }
        public string? Bandeira { get; set; }

        // Navegação
        public VendasEntity Venda { get; set; } = default!;
        public ICollection<MovCaixaEntity> MovimentosCaixa { get; set; } = new List<MovCaixaEntity>();

    }
}
