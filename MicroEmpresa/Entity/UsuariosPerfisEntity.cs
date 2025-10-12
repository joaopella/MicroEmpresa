namespace MicroEmpresa.Entity
{
    public class UsuariosPerfisEntity
    {
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }

        public DateTime CriadoEm { get; set; }
        public byte[] Rv { get; set; } = Array.Empty<byte>();

        // Navegações
        public UsuariosLojaEntity Usuario { get; set; } = default!;
        public PerfisEntity Perfil { get; set; } = default!;
    }
}
