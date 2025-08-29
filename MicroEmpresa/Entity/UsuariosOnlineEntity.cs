namespace MicroEmpresa.Entity
{
    public class UsuariosOnlineEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }

        public string Nome { get; set; } = default!;
        public string? Cpf { get; set; }
        public string Email { get; set; } = default!;
        public string Login { get; set; } = default!;
        public string SenhaHash { get; set; } = default!;

        // Navegação
        public LojasEntity Loja { get; set; } = default!;
    }
}
