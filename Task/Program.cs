using Business_Logic_Layer.Services;
using DomainLayer.Interfaces;
using InfrastructureLayer.DbContextData;
using InfrastructureLayer.Interfaces;
using InfrastructureLayer.Repositories;
using InfrastructureLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()  
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)  
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            builder.Services.AddDbContext<ApplicationDbcontext>(options =>
                options.UseInMemoryDatabase("TodoDb"));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); 
            builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>(); 
            builder.Services.AddScoped<TodoItemService>(); 

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Todo API",
                    Version = "v1",
                    Description = "A simple API for managing Todo items"
                });
            });

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

            Log.CloseAndFlush();
        }
    }
}
