namespace MicroEmpresa.Entity
{
    public class EnderecosEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }

        public string? Tipo { get; set; }              // residencial | comercial | entrega
        public string Logradouro { get; set; } = default!;
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string Cidade { get; set; } = default!;
        public string Uf { get; set; } = default!;     // 2 chars
        public string? Cep { get; set; }

        public ClientesEntity Cliente { get; set; } = default!;
    }
}
