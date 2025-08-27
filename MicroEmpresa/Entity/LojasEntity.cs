namespace MicroEmpresa.Entity
{
    public class LojasEntity : AuditableEntity
    {
        public int Id { get; set; }
        public string NomeFantasia { get; set; } = default!;
        public string? Cnpj { get; set; }
        public string? Telefone { get; set; }

        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Uf { get; set; }
        public string? Cep { get; set; }

        // Navegações (opcional)
        public ICollection<ProdutosEntity> Produtos { get; set; } = new List<ProdutosEntity>();
        public ICollection<ClientesEntity> Clientes { get; set; } = new List<ClientesEntity>();
    }
}
