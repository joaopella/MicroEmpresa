namespace MicroEmpresa.Entity
{
    public class CaixasEntity
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public int IdFuncionarioAbertura { get; set; }
        public int? IdFuncionarioFechamento { get; set; }

        public DateTime DataAbertura { get; set; }
        public decimal ValorInicial { get; set; }
        public DateTime? DataFechamento { get; set; }
        public decimal? ValorFechamento { get; set; }
        public string? Obs { get; set; }

        public byte[] Rv { get; set; } = default!;

        // Navegações
        public LojasEntity Loja { get; set; } = default!;
        public FuncionariosEntity FuncionarioAbertura { get; set; } = default!;
        public FuncionariosEntity? FuncionarioFechamento { get; set; }
        public ICollection<VendasEntity> Vendas { get; set; } = new List<VendasEntity>();
        public ICollection<MovCaixaEntity> Movimentos { get; set; } = new List<MovCaixaEntity>();
    }
}
