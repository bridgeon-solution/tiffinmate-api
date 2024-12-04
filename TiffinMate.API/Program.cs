
using Microsoft.EntityFrameworkCore;
using TiffinMate.DAL.DbContexts;

namespace TiffinMate.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            DotNetEnv.Env.Load();
            var env = Environment.GetEnvironmentVariable("IS_DEVELOPMENT");
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


            var app = builder.Build();
            app.MapGet("/", () => "Testing..!");

            // Configure the HTTP request pipeline.
            if (env == "Development")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHttpsRedirection();
            }



            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
