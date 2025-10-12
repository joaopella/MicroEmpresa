using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration
{
    public class FuncionariosConfiguration : IEntityTypeConfiguration<FuncionariosEntity>
    {
        public void Configure(EntityTypeBuilder<FuncionariosEntity> e)
        {
            e.ToTable("funcionarios", "dbo");
            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.IdLoja).HasColumnName("id_loja").IsRequired();

            e.Property(x => x.Nome).HasColumnName("nome").HasMaxLength(150).IsRequired();
            e.Property(x => x.Cpf).HasColumnName("cpf").HasColumnType("char(11)");
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(150);
            e.Property(x => x.Telefone).HasColumnName("telefone").HasMaxLength(20);

            // -> remove 'cargo' e usa FK para perfis
            e.Property(x => x.IdPerfil).HasColumnName("id_perfil").IsRequired();

            e.Property(x => x.Ativo).HasColumnName("ativo");

            e.Property(x => x.CriadoEm).HasColumnName("criado_em");
            e.Property(x => x.AtualizadoEm).HasColumnName("atualizado_em");
            e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

            e.HasOne(x => x.Loja)
             .WithMany(l => l.Funcionarios)
             .HasForeignKey(x => x.IdLoja)
             .HasConstraintName("FK_funcionarios_loja");

            e.HasOne(x => x.Perfil)
             .WithMany(p => p.Funcionarios)
             .HasForeignKey(x => x.IdPerfil)
             .HasConstraintName("FK_funcionarios_perfis");

            e.HasIndex(x => x.IdPerfil).HasDatabaseName("IX_funcionarios_id_perfil");
        }
    }
}


