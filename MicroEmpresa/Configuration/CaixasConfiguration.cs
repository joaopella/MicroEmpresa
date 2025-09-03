using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration;

public class CaixasConfiguration : IEntityTypeConfiguration<CaixasEntity>
{
    public void Configure(EntityTypeBuilder<CaixasEntity> e)
    {
        e.ToTable("caixas", "dbo");
        e.HasKey(x => x.Id);

        e.Property(x => x.IdLoja).HasColumnName("id_loja").IsRequired();
        e.Property(x => x.IdFuncionarioAbertura).HasColumnName("id_funcionario_abertura").IsRequired();
        e.Property(x => x.IdFuncionarioFechamento).HasColumnName("id_funcionario_fechamento");

        e.Property(x => x.DataAbertura).HasColumnName("data_abertura").IsRequired();
        e.Property(x => x.ValorInicial).HasColumnName("valor_inicial").HasPrecision(18, 2).IsRequired();
        e.Property(x => x.DataFechamento).HasColumnName("data_fechamento");
        e.Property(x => x.ValorFechamento).HasColumnName("valor_fechamento").HasPrecision(18, 2);
        e.Property(x => x.Obs).HasColumnName("obs").HasMaxLength(500);

        e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

        e.HasOne(x => x.Loja)
         .WithMany() // se tiver coleção Caixas em Loja, troque por .WithMany(l => l.Caixas)
         .HasForeignKey(x => x.IdLoja)
         .HasConstraintName("FK_caixas_loja");

        e.HasOne(x => x.FuncionarioAbertura)
         .WithMany()
         .HasForeignKey(x => x.IdFuncionarioAbertura)
         .HasConstraintName("FK_caixas_func_abertura");

        e.HasOne(x => x.FuncionarioFechamento)
         .WithMany()
         .HasForeignKey(x => x.IdFuncionarioFechamento)
         .HasConstraintName("FK_caixas_func_fechamento");
    }
}
