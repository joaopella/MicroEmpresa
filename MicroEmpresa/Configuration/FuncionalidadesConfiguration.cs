using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration
{
    public class FuncionalidadesConfiguration : IEntityTypeConfiguration<FuncionalidadesEntity>
    {
        public void Configure(EntityTypeBuilder<FuncionalidadesEntity> e)
        {
            e.ToTable("funcionalidades", "dbo");
            e.HasKey(x => x.Id);

            e.Property(x => x.Id)
                .HasColumnName("id");

            e.Property(x => x.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(80)
                .IsRequired();

            // (Opcional) evitar nomes duplicados
            e.HasIndex(x => x.Descricao)
             .IsUnique()
             .HasDatabaseName("UX_funcionalidades_descricao");
        }
    }
}
