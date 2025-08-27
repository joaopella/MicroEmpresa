using MicroEmpresa.Entity;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<LojasEntity> Lojas => Set<LojasEntity>();
        public DbSet<ProdutosEntity> Produtos => Set<ProdutosEntity>();
        public DbSet<ClientesEntity> Clientes => Set<ClientesEntity>();
        public DbSet<EnderecosEntity> Enderecos => Set<EnderecosEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // carrega TODAS as classes que implementam IEntityTypeConfiguration<>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
