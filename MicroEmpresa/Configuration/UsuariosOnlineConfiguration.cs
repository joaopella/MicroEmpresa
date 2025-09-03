using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration;

public class UsuariosOnlineConfiguration : IEntityTypeConfiguration<UsuariosOnlineEntity>
{
    public void Configure(EntityTypeBuilder<UsuariosOnlineEntity> e)
    {
        e.ToTable("usuarios_online", "dbo");
        e.HasKey(x => x.Id);

        e.Property(x => x.IdLoja).HasColumnName("id_loja").IsRequired();

        e.Property(x => x.Nome).HasColumnName("nome").HasMaxLength(150).IsRequired();
        e.Property(x => x.Cpf).HasColumnName("cpf").HasColumnType("char(11)");
        e.Property(x => x.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
        e.Property(x => x.Login).HasColumnName("login").HasMaxLength(60).IsRequired();
        e.Property(x => x.SenhaHash).HasColumnName("senha_hash").HasMaxLength(200).IsRequired();

        e.Property(x => x.CriadoEm).HasColumnName("criado_em");
        e.Property(x => x.AtualizadoEm).HasColumnName("atualizado_em");
        e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();

        e.HasOne(x => x.Loja)
         .WithMany()
         .HasForeignKey(x => x.IdLoja)
         .HasConstraintName("FK_usuarios_online_loja");
    }
}
