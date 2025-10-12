namespace MicroEmpresa.Entity
{
    public class PerfisFuncionalidadesEntity : AuditableEntity
    {
        public int IdPerfil { get; set; }
        public int IdFuncao { get; set; }

        // Ordem: C R U D  (X = permitido, 0 = negado)
        public string Crud { get; set; } = "0000";


        // Navegações
        public PerfisEntity Perfil { get; set; } = default!;
        public FuncionalidadesEntity Funcao { get; set; } = default!;
    }
}
