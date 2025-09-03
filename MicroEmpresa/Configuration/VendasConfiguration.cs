using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration;

public class VendasConfiguration : IEntityTypeConfiguration<VendasEntity>
{
    public void Configure(EntityTypeBuilder<VendasEntity> e)
    {
        e.ToTable("vendas", "dbo");
        e.HasKey(x => x.Id);

        e.Property(x => x.IdLoja).HasColumnName("id_loja").IsRequired();
        e.Property(x => x.IdCliente).HasColumnName("id_cliente");
        e.Property(x => x.IdCaixa).HasColumnName("id_caixa");

        e.Property(x => x.DataVenda).HasColumnName("data_venda").IsRequired();
        e.Property(x => x.DescontoTotal).HasColumnName("desconto_total").HasPrecision(18, 2);
        e.Property(x => x.AcrescimoTotal).HasColumnName("acrescimo_total").HasPrecision(18, 2);

        e.Property(x => x.CriadoEm).HasColumnName("criado_em");
        e.Property(x => x.AtualizadoEm).HasColumnName("atualizado_em");
        e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

        e.HasOne(v => v.Loja)
         .WithMany()
         .HasForeignKey(v => v.IdLoja)
         .HasConstraintName("FK_vendas_loja");

        e.HasOne(v => v.Cliente)
         .WithMany()
         .HasForeignKey(v => v.IdCliente)
         .HasConstraintName("FK_vendas_cliente");

        e.HasOne(v => v.Caixa)
         .WithMany(c => c.Vendas)
         .HasForeignKey(v => v.IdCaixa)
         .HasConstraintName("FK_vendas_caixa");
    }
}
