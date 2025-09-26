using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration;

public class ProdutosConfiguration : IEntityTypeConfiguration<ProdutosEntity>
{
    public void Configure(EntityTypeBuilder<ProdutosEntity> e)
    {
        e.ToTable("produtos", "dbo");
        e.HasKey(x => x.Id);

        e.Property(x => x.IdLoja).HasColumnName("id_loja").IsRequired();

        e.Property(x => x.Nome).HasColumnName("nome").HasMaxLength(150).IsRequired();
        e.Property(x => x.Sku).HasColumnName("sku").HasMaxLength(60);
        e.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(10).IsRequired();
        e.Property(x => x.Unidade).HasColumnName("unidade").HasMaxLength(10);
        e.Property(x => x.PrecoVenda).HasColumnName("preco_venda").HasPrecision(18, 2);
        e.Property(x => x.Custo).HasColumnName("custo").HasPrecision(18, 2);
        e.Property(x => x.Ativo).HasColumnName("ativo");
        e.Property(x => x.MarkupPercentual).HasColumnName("markup_percentual").HasPrecision(9, 2);
        e.Property(x => x.PrecoSugerido).HasColumnName("preco_sugerido").HasPrecision(18, 2);

        // auditoria/concorrência (vem da AuditableEntity)
        e.Property(x => x.CriadoEm).HasColumnName("criado_em");
        e.Property(x => x.AtualizadoEm).HasColumnName("atualizado_em");
        e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

        // FK
        e.HasOne(x => x.Loja)
         .WithMany(l => l.Produtos)
         .HasForeignKey(x => x.IdLoja)
         .HasConstraintName("FK_produtos_loja");

        e.HasIndex(x => new { x.IdLoja, x.Sku })
         .HasDatabaseName("UX_produtos_loja_sku")
         .IsUnique()
         .HasFilter("[sku] IS NOT NULL");
    }
}
