namespace MicroEmpresa.Entity
{
    public class MovCaixaEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int IdCaixa { get; set; }
        public int? IdPagamento { get; set; }    // quando origem = VENDA

        public string Tipo { get; set; } = default!;     // ENTRADA | SAIDA
        public string Origem { get; set; } = default!;   // ABERTURA | VENDA | SANGRIA | SUPRIMENTO | AJUSTE
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataMov { get; set; }

        // Navegações
        public CaixasEntity Caixa { get; set; } = default!;
        public PagamentosEntity? Pagamento { get; set; }
    }
}
