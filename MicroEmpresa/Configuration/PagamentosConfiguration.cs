using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration;

public class PagamentosConfiguration : IEntityTypeConfiguration<PagamentosEntity>
{
    public void Configure(EntityTypeBuilder<PagamentosEntity> e)
    {
        e.ToTable("pagamentos", "dbo");
        e.HasKey(x => x.Id);

        e.Property(x => x.IdVenda).HasColumnName("id_venda").IsRequired();

        e.Property(x => x.FormaPagamento).HasColumnName("forma_pagamento").HasMaxLength(30).IsRequired();
        e.Property(x => x.Valor).HasColumnName("valor").HasPrecision(18, 2).IsRequired();

        e.Property(x => x.Nsu).HasColumnName("nsu").HasMaxLength(60);
        e.Property(x => x.Autorizacao).HasColumnName("autorizacao").HasMaxLength(40);
        e.Property(x => x.Bandeira).HasColumnName("bandeira").HasMaxLength(20);

        e.Property(x => x.CriadoEm).HasColumnName("criado_em");
        e.Property(x => x.AtualizadoEm).HasColumnName("atualizado_em");
        e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

        e.HasOne(p => p.Venda)
         .WithMany(v => v.Pagamentos)
         .HasForeignKey(p => p.IdVenda)
         .HasConstraintName("FK_pagamentos_venda");
    }
}
