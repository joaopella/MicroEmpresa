namespace MicroEmpresa.Entity
{
    public class FuncionariosEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public string Nome { get; set; } = default!;
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Cargo { get; set; }             // caixa, gerente, atendente...
        public bool Ativo { get; set; }

        // Navegação
        public LojasEntity Loja { get; set; } = default!;
    }
}
