namespace MicroEmpresa.Entity
{
    public class ClientesEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }

        public string Nome { get; set; } = default!;
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }

        public LojasEntity Loja { get; set; } = default!;
        public ICollection<EnderecosEntity> Enderecos { get; set; } = new List<EnderecosEntity>();
    }
}
