using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration
{
    public class EnderecosConfiguration : IEntityTypeConfiguration<EnderecosEntity>
    {
        public void Configure(EntityTypeBuilder<EnderecosEntity> e)
        {
            e.ToTable("enderecos", "dbo");
            e.HasKey(x => x.Id);

            e.Property(x => x.IdCliente).HasColumnName("id_cliente").IsRequired();

            e.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(30);
            e.Property(x => x.Logradouro).HasColumnName("logradouro").HasMaxLength(150).IsRequired();
            e.Property(x => x.Numero).HasColumnName("numero").HasMaxLength(20);
            e.Property(x => x.Complemento).HasColumnName("complemento").HasMaxLength(80);
            e.Property(x => x.Bairro).HasColumnName("bairro").HasMaxLength(80);
            e.Property(x => x.Cidade).HasColumnName("cidade").HasMaxLength(80).IsRequired();
            e.Property(x => x.Uf).HasColumnName("uf").HasColumnType("char(2)").IsRequired();
            e.Property(x => x.Cep).HasColumnName("cep").HasMaxLength(10);

            e.Property(x => x.CriadoEm).HasColumnName("criado_em");
            e.Property(x => x.AtualizadoEm).HasColumnName("atualizado_em");
            e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

            e.HasOne(x => x.Cliente)
             .WithMany(c => c.Enderecos)
             .HasForeignKey(x => x.IdCliente)
             .HasConstraintName("FK_enderecos_clientes");
        }
    }
}
