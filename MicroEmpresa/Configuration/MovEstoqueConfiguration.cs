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

        e.Property(x => x.IdLoja).HasColumnName("id_loja").IsRequired();
        e.Property(x => x.IdProduto).HasColumnName("id_produto").IsRequired();

        e.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(20).IsRequired(); // "entrada" | "saida"
        e.Property(x => x.Qtd).HasColumnName("qtd").HasPrecision(18, 3).IsRequired();
        e.Property(x => x.CustoUnit).HasColumnName("custo_unit").HasPrecision(18, 4);
        e.Property(x => x.Motivo).HasColumnName("motivo").HasMaxLength(200);
        e.Property(x => x.DataMov).HasColumnName("data_mov").IsRequired();

        e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

        e.HasOne(x => x.Loja)
            .WithMany() // se tiver ICollection<MovEstoqueEntity> em LojasEntity, troque por .WithMany(l => l.MovimentosEstoque)
            .HasForeignKey(x => x.IdLoja)
            .HasConstraintName("FK_mov_estoque_loja");

        e.HasOne(x => x.Produto)
            .WithMany() // se tiver ICollection<MovEstoqueEntity> em ProdutosEntity, troque por .WithMany(p => p.MovimentosEstoque)
            .HasForeignKey(x => x.IdProduto)
            .HasConstraintName("FK_mov_estoque_produto");
    }
}
