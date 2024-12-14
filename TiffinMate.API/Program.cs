
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Supabase;
using TiffinMate.BLL.Interfaces.AdminInterface;
using TiffinMate.BLL.Services.AdminService;
//using TiffinMate.API.Middlewares;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Interfaces.AdminInterfaces;
using TiffinMate.DAL.Interfaces.UserRepositoryInterface;
using TiffinMate.BLL.Services.UserService;
using TiffinMate.BLL.Services.AuthService;
using TiffinMate.BLL.Interfaces.AuthInterface;
using TiffinMate.BLL.Mapper;
using TiffinMate.DAL.Repositories.UserRepositories;
using TiffinMate.DAL.Repositories.AdminRepositories;
using TiffinMate.DAL.Interfaces.ProviderInterface;
using TiffinMate.DAL.Repositories.ProviderRepositories;
using TiffinMate.BLL.Interfaces.ProviderServiceInterafce;
using TiffinMate.BLL.Services.ProviderServices;
using TiffinMate.BLL.Interfaces.CloudinaryInterface;
using TiffinMate.BLL.Services.CoudinaryService;
using TiffinMate.DAL.Entities;
using TiffinMate.BLL.Interfaces.UserInterfaces;
using TiffinMate.BLL.Services.UserServices;
using TiffinMate.DAL.Interfaces.UserInterfaces;




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
            builder.Services.AddScoped<AppDbContext>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
            

            builder.Services.AddScoped<ICloudinaryService, CloudinaryServices>();
            builder.Services.Configure<BrevoSettings>(builder.Configuration.GetSection("Brevo"));




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
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

           
           
           
            
    

            builder.Services.AddSingleton<IOtpService>(provider =>
            {
                var accountSid = builder.Configuration["Twilio:Sid"];
                var authToken = builder.Configuration["Twilio:Token"];
                var verifySid = builder.Configuration["Twilio:verifySid"];

                return new OtpService(accountSid, authToken, verifySid);
            });





            var app = builder.Build();
           

            if (env == "Development")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHttpsRedirection();
            }
            //app.UseMiddleware<LoggingMiddleware>();



            
            app.UseCors("AllowAllOrigins");




            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}