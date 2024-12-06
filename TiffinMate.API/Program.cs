using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Supabase;
using TiffinMate.BLL.Interfaces.AdminInterface;
using TiffinMate.BLL.Services.AdminService;
using TiffinMate.API.Middlewares;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Interfaces.AdminInterface;
using TiffinMate.DAL.Repositories.AdminRepository;

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
            builder.Services.AddScoped<AppDbContext>();
            builder.Services.AddScoped<IAdminRepository,AdminRepository>();
            builder.Services.AddScoped<IAdminService,AdminService>();



            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

          

            builder.Services.AddScoped<Supabase.Client>(_ => new Supabase.Client(builder.Configuration.GetConnectionString("HostUrl"), builder.Configuration.GetConnectionString("HostAPI"),
             new SupabaseOptions
             {
                 AutoRefreshToken = true,
                 AutoConnectRealtime = true,
             }));
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])),
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false 
                };
            });


            var app = builder.Build();
            app.UseCors("AllowAllOrigins");
            // Configure the HTTP request pipeline.
            if (env == "Development")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHttpsRedirection();
            }
            app.UseMiddleware<LoggingMiddleware>();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
