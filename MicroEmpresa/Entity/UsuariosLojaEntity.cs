using System.ComponentModel.DataAnnotations.Schema;

namespace MicroEmpresa.Entity
{
    public class UsuariosLojaEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public int IdFuncionario { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }

        [NotMapped]
        public string? Cnpj { get; set; }
    }
}
