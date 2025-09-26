namespace MicroEmpresa.Entity
{
    public class UsuariosOnlineEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public int? IdCliente { get; set; }

        public string Login { get; set; } = default!;
        public byte[] SenhaHash { get; set; } = Array.Empty<byte>(); // varbinary(256)
        public string? Email { get; set; }
        public string? Nome { get; set; }
        public bool Ativo { get; set; }

        // Navegação (opcionais)
        public LojasEntity? Loja { get; set; }
        public ClientesEntity? Cliente { get; set; }
    }
}
