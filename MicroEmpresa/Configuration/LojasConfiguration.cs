using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration;

public class LojasConfiguration : IEntityTypeConfiguration<LojasEntity>
{
    public void Configure(EntityTypeBuilder<LojasEntity> e)
    {
        e.ToTable("lojas", "dbo");
        e.HasKey(x => x.Id);

        e.Property(x => x.NomeFantasia).HasColumnName("nome_fantasia").HasMaxLength(150).IsRequired();
        e.Property(x => x.Cnpj).HasColumnName("cnpj").HasColumnType("char(14)");
        e.Property(x => x.Telefone).HasColumnName("telefone").HasMaxLength(20);

        e.Property(x => x.CriadoEm).HasColumnName("criado_em");
        e.Property(x => x.AtualizadoEm).HasColumnName("atualizado_em");
        e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

        // Navegações (só funcionam se as coleções existirem na entidade)
        e.HasMany(l => l.Produtos)
         .WithOne(p => p.Loja)
         .HasForeignKey(p => p.IdLoja)
         .HasConstraintName("FK_produtos_loja");

        e.HasMany(l => l.Clientes)
         .WithOne(c => c.Loja)
         .HasForeignKey(c => c.IdLoja)
         .HasConstraintName("FK_clientes_loja");

        e.HasMany(l => l.Funcionarios)
         .WithOne(f => f.Loja)
         .HasForeignKey(f => f.IdLoja)
         .HasConstraintName("FK_funcionarios_loja");
    }
}
