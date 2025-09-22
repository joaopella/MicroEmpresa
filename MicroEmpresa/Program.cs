
using MicroEmpresa.Date;
using MicroEmpresa.Logic;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // EF Core + SQL Server
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped<ILojasRepository, LojasData>();
            builder.Services.AddScoped<IEnderecosRepository, EnderecosData>();
            builder.Services.AddScoped<IClientesRepository, ClientesData>(); // se tiver

            // Services (Logic)
            builder.Services.AddScoped<MicroEmpresa.Logic.Lojas.ILojasLogic, MicroEmpresa.Logic.Lojas.LojasLogic>();
            builder.Services.AddScoped<IEnderecosLogic, EnderecosLogic>();
            builder.Services.AddScoped<IClientesService, ClientesService>();

            builder.Services.AddControllers();

            // Swagger (Swashbuckle)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
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
