namespace MicroEmpresa.Entity
{
    public class AuditableEntity
    {
        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
        public byte[]? Rv { get; set; }
    }
}
