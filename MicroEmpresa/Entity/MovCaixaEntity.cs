namespace MicroEmpresa.Entity
{
    public class MovCaixaEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int IdCaixa { get; set; }
        public int? IdPagamento { get; set; }

        public string Tipo { get; set; } = default!;    // entrada, saída, etc
        public string Origem { get; set; } = default!;  // venda, ajuste, etc
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataMov { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }

        public byte[] Rv { get; set; } = default!;

        // navegações
        public CaixasEntity Caixa { get; set; } = default!;
        public PagamentosEntity? Pagamento { get; set; }
    }
}
