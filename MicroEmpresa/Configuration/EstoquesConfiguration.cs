using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration;

public class EstoquesConfiguration : IEntityTypeConfiguration<EstoquesEntity>
{
    public void Configure(EntityTypeBuilder<EstoquesEntity> e)
    {
        e.ToTable("estoques", "dbo");
        e.HasKey(x => x.Id);

        e.Property(x => x.IdLoja).HasColumnName("id_loja").IsRequired();
        e.Property(x => x.IdProduto).HasColumnName("id_produto").IsRequired();
        e.Property(x => x.Saldo).HasColumnName("saldo").HasPrecision(18, 3).IsRequired();

        e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

        e.HasOne(x => x.Loja)
         .WithMany()
         .HasForeignKey(x => x.IdLoja)
         .HasConstraintName("FK_estoques_loja");

        e.HasOne(x => x.Produto)
         .WithMany()
         .HasForeignKey(x => x.IdProduto)
         .HasConstraintName("FK_estoques_produto");
    }
}
