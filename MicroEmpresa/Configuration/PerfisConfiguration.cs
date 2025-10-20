using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration
{
    public class PerfisConfiguration : IEntityTypeConfiguration<PerfisEntity>
    {
        public void Configure(EntityTypeBuilder<PerfisEntity> e)
        {
            e.ToTable("perfis", "dbo");
            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id");

            e.Property(x => x.Nome)
                .HasColumnName("nome")
                .HasMaxLength(80)
                .IsRequired();

            e.Property(x => x.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(200);

            // >>> Datas geradas pelo banco
            e.Property(x => x.CriadoEm)
             .HasColumnName("criado_em")
             .HasColumnType("datetime2(0)")
             .HasDefaultValueSql("SYSUTCDATETIME()") // default no INSERT
             .ValueGeneratedOnAdd();

            e.Property(x => x.AtualizadoEm)
             .HasColumnName("atualizado_em")
             .HasColumnType("datetime2(0)")
             .HasDefaultValueSql("SYSUTCDATETIME()") // default no INSERT
             .ValueGeneratedOnAddOrUpdate();         // trigger atualiza no UPDATE

            e.Property(x => x.Rv)
             .HasColumnName("rv")
             .IsRowVersion()
             .IsConcurrencyToken();

            e.HasIndex(x => x.Nome)
             .IsUnique()
             .HasDatabaseName("UQ_perfis_nome");
        }
    }
}
