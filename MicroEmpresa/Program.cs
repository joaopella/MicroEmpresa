using MicroEmpresa.Date;
using MicroEmpresa.Logic;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace MicroEmpresa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            // ===== Repositories =====
            builder.Services.AddScoped<ILojasRepository, LojasData>();
            builder.Services.AddScoped<IEnderecosRepository, EnderecosData>();
            builder.Services.AddScoped<IClientesRepository, ClientesData>();
            builder.Services.AddScoped<IEstoquesRepository, EstoquesData>();
            builder.Services.AddScoped<ICaixasRepository, CaixasData>();
            builder.Services.AddScoped<IMovCaixaRepository, MovCaixaData>();
            builder.Services.AddScoped<IMovEstoqueRepository, MovEstoqueData>();
            builder.Services.AddScoped<IPagamentosRepository, PagamentosData>();
            builder.Services.AddScoped<IProdutosRepository, ProdutosData>();
            builder.Services.AddScoped<IUsuariosOnlineRepository, UsuariosOnlineData>();

            // ===== Logic/Services =====
            builder.Services.AddScoped<ILojasLogic, LojasLogic>();
            builder.Services.AddScoped<IEnderecosLogic, EnderecosLogic>();
            builder.Services.AddScoped<IClientesLogic, ClientesLogic>();
            builder.Services.AddScoped<IEstoquesLogic, EstoquesLogic>();
            builder.Services.AddScoped<ICaixasLogic, CaixasLogic>();
            builder.Services.AddScoped<IMovCaixaLogic, MovCaixaLogic>();
            builder.Services.AddScoped<IMovEstoqueLogic, MovEstoqueLogic>();
            builder.Services.AddScoped<IPagamentosLogic, PagamentosLogic>();
            builder.Services.AddScoped<IProdutosLogic, ProdutosLogic>();
            builder.Services.AddScoped<IUsuariosOnlineLogic, UsuariosOnlineLogic>();

            // Controllers + JSON (evita ciclos de navegação do EF)
            builder.Services.AddControllers()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            // Swagger (Swashbuckle) — evita conflito de SchemaId
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroEmpresa API", Version = "v1" });
                c.CustomSchemaIds(t => t.FullName); // <- chave para resolver o 500
            });

            var app = builder.Build();

            // Se quiser Swagger também em produção, deixe sem if
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
