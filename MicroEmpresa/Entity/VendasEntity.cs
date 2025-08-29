namespace MicroEmpresa.Entity
{
    public class VendasEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public int? IdCliente { get; set; }
        public int? IdCaixa { get; set; }

        public DateTime DataVenda { get; set; }
        public decimal? DescontoTotal { get; set; }
        public decimal? AcrescimoTotal { get; set; }

        // Navegações
        public LojasEntity Loja { get; set; } = default!;
        public ClientesEntity? Cliente { get; set; }
        public CaixasEntity? Caixa { get; set; }
        public ICollection<VendasItensEntity> Itens { get; set; } = new List<VendasItensEntity>();
        public ICollection<PagamentosEntity> Pagamentos { get; set; } = new List<PagamentosEntity>();
    }
}
