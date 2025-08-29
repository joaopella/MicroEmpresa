namespace MicroEmpresa.Entity
{
    public class EnderecosEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int? IdCliente { get; set; }
        public int? IdLoja { get; set; }

        public string? Tipo { get; set; }
        public string Logradouro { get; set; } = default!;
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string Cidade { get; set; } = default!;
        public string Uf { get; set; } = default!;
        public string? Cep { get; set; }

        // Navegações
        public ClientesEntity? Cliente { get; set; }
        public LojasEntity? Loja { get; set; }
    }

}
