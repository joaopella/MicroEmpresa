namespace MicroEmpresa.Entity
{
    public class PerfisEntity : AuditableEntity
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }

        // Navegações (opcionais – use se quiser acessar relacionados)
        public ICollection<FuncionariosEntity> Funcionarios { get; set; } = new List<FuncionariosEntity>();
        public ICollection<PerfisFuncionalidadesEntity> Permissoes { get; set; } = new List<PerfisFuncionalidadesEntity>();
        public ICollection<UsuariosPerfisEntity> Usuarios { get; set; } = new List<UsuariosPerfisEntity>();
    }
}
