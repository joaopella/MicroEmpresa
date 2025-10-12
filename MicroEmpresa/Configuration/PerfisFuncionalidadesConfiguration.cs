using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration
{
    public class PerfisFuncionalidadesConfiguration : IEntityTypeConfiguration<PerfisFuncionalidadesEntity>
    {
        public void Configure(EntityTypeBuilder<PerfisFuncionalidadesEntity> e)
        {
            e.ToTable("perfis_funcionalidades", "dbo");

            // PK composta
            e.HasKey(x => new { x.IdPerfil, x.IdFuncao });

            // Colunas
            e.Property(x => x.IdPerfil).HasColumnName("id_perfil").IsRequired();
            e.Property(x => x.IdFuncao).HasColumnName("id_funcao").IsRequired();

            e.Property(x => x.Crud)
                .HasColumnName("crud")
                .HasColumnType("char(4)")
                .HasDefaultValue("0000")
                .IsRequired();

            e.Property(x => x.CriadoEm)
                .HasColumnName("criado_em")
                .HasColumnType("datetime2(0)")
                .IsRequired();

            e.Property(x => x.AtualizadoEm)
                .HasColumnName("atualizado_em")
                .HasColumnType("datetime2(0)")
                .IsRequired();

            // FKs
            e.HasOne(x => x.Perfil)
             .WithMany(p => p.Permissoes)
             .HasForeignKey(x => x.IdPerfil)
             .HasConstraintName("FK_pf_perfil");

            e.HasOne(x => x.Funcao)
             .WithMany(f => f.PerfisVinculos)
             .HasForeignKey(x => x.IdFuncao)
             .HasConstraintName("FK_pf_funcao");

            // Índice auxiliar para buscas por função
            e.HasIndex(x => x.IdFuncao)
             .HasDatabaseName("IX_perfis_funcionalidades_id_funcao");
        }
    }
}
