using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Mappings;
using TaskManager.Core.Services;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;
using TaskManager.Infrastructure.Seed;

namespace TaskManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("Tasks")
            );

            // UnitOfWork and Repository
            builder.Services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(StatusMappingProfile), typeof(TaskMappingProfile));

            // Services 
            builder.Services.AddScoped<ITaskService, TaskService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Seeder треба викликати після створення app
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DatabaseSeeder.Seed(dbContext);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
