using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration
{
    public class UsuariosLojaConfiguration : IEntityTypeConfiguration<UsuariosLojaEntity>
    {
        public void Configure(EntityTypeBuilder<UsuariosLojaEntity> e)
        {
            e.ToTable("usuarios_loja", "dbo");
            e.HasKey(x => x.Id);

            e.Property(x => x.IdLoja)
             .HasColumnName("id_loja")
             .IsRequired();

            e.Property(x => x.IdFuncionario)
             .HasColumnName("id_funcionario")
             .IsRequired();

            e.Property(x => x.Login)
             .HasColumnName("login")
             .HasMaxLength(100)
             .IsRequired();

            e.Property(x => x.Email)
             .HasColumnName("email")
             .HasMaxLength(200);

            e.Property(x => x.Senha)
             .HasColumnName("senha_hash")
             .HasMaxLength(255)
             .IsRequired();

            e.Property(x => x.CriadoEm)
             .HasColumnName("criado_em")
             .HasDefaultValueSql("SYSDATETIME()");

            e.Property(x => x.AtualizadoEm)
             .HasColumnName("atualizado_em");

            e.Property(x => x.Rv)
             .HasColumnName("rv")
             .IsRowVersion()
             .IsConcurrencyToken();

            e.HasOne<LojasEntity>()
             .WithMany()
             .HasForeignKey(x => x.IdLoja)
             .HasConstraintName("FK_usuario_loja_lojas");

            e.HasOne<FuncionariosEntity>()
             .WithMany()
             .HasForeignKey(x => x.IdFuncionario)
             .HasConstraintName("FK_usuario_loja_funcionarios");

            // Índices
            e.HasIndex(x => x.IdLoja)
             .HasDatabaseName("IX_usuarios_loja_id_loja");

            e.HasIndex(x => x.IdFuncionario)
             .HasDatabaseName("IX_usuarios_loja_id_funcionario");

            e.HasIndex(x => new { x.IdLoja, x.Login })
             .HasDatabaseName("UX_usuarios_loja_loja_login")
             .IsUnique();

            e.HasIndex(x => new { x.IdLoja, x.IdFuncionario })
             .HasDatabaseName("UX_usuarios_loja_loja_funcionario")
             .IsUnique();
        }
    }

}
