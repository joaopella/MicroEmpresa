using System.ComponentModel.DataAnnotations.Schema;

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

        public bool Ativo { get; set; }

        // FK para perfis
        public int IdPerfil { get; set; }

        // Navegações
        public LojasEntity? Loja { get; set; }
        public PerfisEntity? Perfil { get; set; } 

        // Só para input/consulta (não existe na tabela)
        [NotMapped]
        public string? Cnpj { get; set; }
        [NotMapped] 
        public string? PerfilNome { get; set; }


    }
}
