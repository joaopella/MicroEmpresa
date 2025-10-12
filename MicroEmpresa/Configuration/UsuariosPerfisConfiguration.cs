using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration
{
    public class UsuariosPerfisConfiguration : IEntityTypeConfiguration<UsuariosPerfisEntity>
    {
        public void Configure(EntityTypeBuilder<UsuariosPerfisEntity> e)
        {
            e.ToTable("usuarios_perfis", "dbo");

            // PK composta
            e.HasKey(x => new { x.IdUsuario, x.IdPerfil });

            // Colunas
            e.Property(x => x.IdUsuario).HasColumnName("id_usuario").IsRequired();
            e.Property(x => x.IdPerfil).HasColumnName("id_perfil").IsRequired();

            e.Property(x => x.CriadoEm)
                .HasColumnName("criado_em")
                .HasColumnType("datetime2(0)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            e.Property(x => x.Rv)
                .HasColumnName("rv")
                .IsRowVersion()
                .IsConcurrencyToken();

            // FKs
            e.HasOne(x => x.Usuario)
             .WithMany() // ou .WithMany(u => u.Perfis) se tiver coleção em UsuariosLojaEntity
             .HasForeignKey(x => x.IdUsuario)
             .HasConstraintName("FK_usuariosperfis_usuario");

            e.HasOne(x => x.Perfil)
             .WithMany(p => p.Usuarios)
             .HasForeignKey(x => x.IdPerfil)
             .HasConstraintName("FK_usuariosperfis_perfil");

            // Índices auxiliares (opcionais)
            e.HasIndex(x => x.IdPerfil).HasDatabaseName("IX_usuarios_perfis_id_perfil");
        }
    }
}
