
using Microsoft.EntityFrameworkCore;
using Supabase;
using TiffinMate.API.Middlewares;
using TiffinMate.DAL.DbContexts;

namespace TiffinMate.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            DotNetEnv.Env.Load();
            var env = Environment.GetEnvironmentVariable("IS_DEVELOPMENT");
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<Supabase.Client>(_ => new Supabase.Client(builder.Configuration.GetConnectionString("HostUrl"), builder.Configuration.GetConnectionString("HostAPI"),
             new SupabaseOptions
             {
                 AutoRefreshToken = true,
                 AutoConnectRealtime = true,
             }));
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


            var app = builder.Build();
            app.MapGet("/", () => "Hello World!");

            // Configure the HTTP request pipeline.
            if (env == "Development")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHttpsRedirection();
            }
            app.UseMiddleware<LoggingMiddleware>();


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
