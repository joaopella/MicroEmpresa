using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration;

public class VendasItensConfiguration : IEntityTypeConfiguration<VendasItensEntity>
{
    public void Configure(EntityTypeBuilder<VendasItensEntity> e)
    {
        e.ToTable("vendas_itens", "dbo");
        e.HasKey(x => x.Id);

        e.Property(x => x.IdVenda).HasColumnName("id_venda").IsRequired();
        e.Property(x => x.IdProduto).HasColumnName("id_produto").IsRequired();

        e.Property(x => x.Quantidade).HasColumnName("quantidade").HasPrecision(18, 3).IsRequired();
        e.Property(x => x.PrecoUnit).HasColumnName("preco_unit").HasPrecision(18, 2).IsRequired();
        e.Property(x => x.DescontoItem).HasColumnName("desconto_item").HasPrecision(18, 2);
        e.Property(x => x.AcrescimoItem).HasColumnName("acrescimo_item").HasPrecision(18, 2);

        e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

        e.HasOne(x => x.Venda)
         .WithMany(v => v.Itens)
         .HasForeignKey(x => x.IdVenda)
         .HasConstraintName("FK_vendas_itens_venda");

        e.HasOne(x => x.Produto)
         .WithMany()
         .HasForeignKey(x => x.IdProduto)
         .HasConstraintName("FK_vendas_itens_produto");
    }
}
