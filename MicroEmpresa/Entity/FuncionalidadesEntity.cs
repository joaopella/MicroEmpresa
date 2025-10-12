namespace MicroEmpresa.Entity
{
    public class FuncionalidadesEntity
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;

        // Navegação opcional para a tabela de junção
        public ICollection<PerfisFuncionalidadesEntity> PerfisVinculos { get; set; } = new List<PerfisFuncionalidadesEntity>();
    }
}
