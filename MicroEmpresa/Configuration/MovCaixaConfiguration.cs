using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration;

public class MovCaixaConfiguration : IEntityTypeConfiguration<MovCaixaEntity>
{
    public void Configure(EntityTypeBuilder<MovCaixaEntity> e)
    {
        e.ToTable("mov_caixa", "dbo");
        e.HasKey(x => x.Id);

        e.Property(x => x.IdCaixa).HasColumnName("id_caixa").IsRequired();
        e.Property(x => x.IdPagamento).HasColumnName("id_pagamento");

        e.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(10).IsRequired();       // ENTRADA|SAIDA
        e.Property(x => x.Origem).HasColumnName("origem").HasMaxLength(20).IsRequired();   // ABERTURA|VENDA|...
        e.Property(x => x.Descricao).HasColumnName("descricao").HasMaxLength(200);
        e.Property(x => x.Valor).HasColumnName("valor").HasPrecision(18, 2).IsRequired();
        e.Property(x => x.DataMov).HasColumnName("data_mov").IsRequired();

        e.Property(x => x.CriadoEm).HasColumnName("criado_em");
        e.Property(x => x.AtualizadoEm).HasColumnName("atualizado_em");
        e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

        e.HasOne(x => x.Caixa)
         .WithMany(c => c.Movimentos)
         .HasForeignKey(x => x.IdCaixa)
         .HasConstraintName("FK_mov_caixa_caixa");

        e.HasOne(x => x.Pagamento)
         .WithMany()
         .HasForeignKey(x => x.IdPagamento)
         .HasConstraintName("FK_mov_caixa_pagamento");
    }
}
