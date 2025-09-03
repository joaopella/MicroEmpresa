using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration;

public class MovEstoqueConfiguration : IEntityTypeConfiguration<MovEstoqueEntity>
{
    public void Configure(EntityTypeBuilder<MovEstoqueEntity> e)
    {
        e.ToTable("mov_estoque", "dbo");
        e.HasKey(x => x.Id);

        e.Property(x => x.IdEstoque).HasColumnName("id_estoque").IsRequired();
        e.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(10).IsRequired();   // ENTRADA|SAIDA
        e.Property(x => x.Quantidade).HasColumnName("quantidade").HasPrecision(18, 3).IsRequired();
        e.Property(x => x.DataMov).HasColumnName("data_mov").IsRequired();
        e.Property(x => x.Obs).HasColumnName("obs").HasMaxLength(200);

        e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

        e.HasOne(x => x.Estoque)
         .WithMany(e0 => e0.Movimentos)
         .HasForeignKey(x => x.IdEstoque)
         .HasConstraintName("FK_mov_estoque_estoque");
    }
}
