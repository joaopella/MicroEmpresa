using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration
{
    public class ClientesConfiguration : IEntityTypeConfiguration<ClientesEntity>
    {
        public void Configure(EntityTypeBuilder<ClientesEntity> e)
        {
            e.ToTable("clientes", "dbo");
            e.HasKey(x => x.Id);

            e.Property(x => x.IdLoja).HasColumnName("id_loja").IsRequired();

            e.Property(x => x.Nome).HasColumnName("nome").HasMaxLength(150).IsRequired();
            e.Property(x => x.Cpf).HasColumnName("cpf").HasColumnType("char(11)");
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(150);
            e.Property(x => x.Telefone).HasColumnName("telefone").HasMaxLength(20);

            e.Property(x => x.CriadoEm).HasColumnName("criado_em");
            e.Property(x => x.AtualizadoEm).HasColumnName("atualizado_em");
            e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

            e.HasOne(x => x.Loja)
             .WithMany(x => x.Clientes)
             .HasForeignKey(x => x.IdLoja)
             .HasConstraintName("FK_clientes_loja");
        }
    }
}
