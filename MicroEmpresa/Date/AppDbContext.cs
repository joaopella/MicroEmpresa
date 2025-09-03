using Microsoft.EntityFrameworkCore;
using MicroEmpresa.Entity;

namespace MicroEmpresa.Date
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        // DbSets (tabelas)
        public DbSet<LojasEntity> Lojas => Set<LojasEntity>();
        public DbSet<ProdutosEntity> Produtos => Set<ProdutosEntity>();
        public DbSet<ClientesEntity> Clientes => Set<ClientesEntity>();
        public DbSet<EnderecosEntity> Enderecos => Set<EnderecosEntity>();
        public DbSet<FuncionariosEntity> Funcionarios => Set<FuncionariosEntity>();
        public DbSet<CaixasEntity> Caixas => Set<CaixasEntity>();
        public DbSet<VendasEntity> Vendas => Set<VendasEntity>();
        public DbSet<VendasItensEntity> VendasItens => Set<VendasItensEntity>();
        public DbSet<PagamentosEntity> Pagamentos => Set<PagamentosEntity>();
        public DbSet<MovCaixaEntity> MovimentosCaixa => Set<MovCaixaEntity>();
        public DbSet<EstoquesEntity> Estoques => Set<EstoquesEntity>();
        public DbSet<MovEstoqueEntity> MovimentosEstoque => Set<MovEstoqueEntity>();
        public DbSet<UsuariosOnlineEntity> UsuariosOnline => Set<UsuariosOnlineEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // aplica automaticamente TODAS as IEntityTypeConfiguration<> do projeto
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
