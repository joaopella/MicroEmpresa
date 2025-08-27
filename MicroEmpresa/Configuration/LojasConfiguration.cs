using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroEmpresa.Configuration
{
    public class LojasConfiguration : IEntityTypeConfiguration<LojasEntity>
    {
        public void Configure(EntityTypeBuilder<LojasEntity> e)
        {
            e.ToTable("lojas", "dbo");
            e.HasKey(x => x.Id);

            e.Property(x => x.NomeFantasia).HasColumnName("nome_fantasia").HasMaxLength(150).IsRequired();
            e.Property(x => x.Cnpj).HasColumnName("cnpj").HasColumnType("char(14)");
            e.Property(x => x.Telefone).HasColumnName("telefone").HasMaxLength(20);

            e.Property(x => x.Logradouro).HasColumnName("logradouro").HasMaxLength(150);
            e.Property(x => x.Numero).HasColumnName("numero").HasMaxLength(20);
            e.Property(x => x.Complemento).HasColumnName("complemento").HasMaxLength(80);
            e.Property(x => x.Bairro).HasColumnName("bairro").HasMaxLength(80);
            e.Property(x => x.Cidade).HasColumnName("cidade").HasMaxLength(80);
            e.Property(x => x.Uf).HasColumnName("uf").HasColumnType("char(2)");
            e.Property(x => x.Cep).HasColumnName("cep").HasMaxLength(10);

            e.Property(x => x.CriadoEm).HasColumnName("criado_em");
            e.Property(x => x.AtualizadoEm).HasColumnName("atualizado_em");
            e.Property(x => x.Rv).HasColumnName("rv").IsRowVersion().IsConcurrencyToken();
        }
    }
}
