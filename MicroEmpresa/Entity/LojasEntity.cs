namespace MicroEmpresa.Entity
{
    public class LojasEntity : AuditableEntity
    {
        public int Id { get; set; }
        public string NomeFantasia { get; set; } = default!;
        public string? Cnpj { get; set; }
        public string? Telefone { get; set; }

        // Navegações
        public ICollection<EnderecosEntity> Enderecos { get; set; } = new List<EnderecosEntity>();
        public ICollection<ProdutosEntity> Produtos { get; set; } = new List<ProdutosEntity>();
        public ICollection<ClientesEntity> Clientes { get; set; } = new List<ClientesEntity>();
        public ICollection<FuncionariosEntity> Funcionarios { get; set; } = new List<FuncionariosEntity>();
    }
}
