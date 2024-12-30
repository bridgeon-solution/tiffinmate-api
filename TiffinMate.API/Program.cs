
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
using TiffinMate.BLL.Interfaces.ProviderVerification;
using TiffinMate.BLL.Services.ProviderVerification;
using TiffinMate.BLL.Interfaces.UserInterfaces;
using TiffinMate.BLL.Services.UserServices;
using TiffinMate.DAL.Interfaces.UserInterfaces;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using TiffinMate.DAL.Interfaces.ReviewInterface;
using TiffinMate.DAL.Repositories.ReviewRepository;
using TiffinMate.DAL.Interfaces.NotificationInterfaces;
using TiffinMate.DAL.Repositories.NotificationRepository;
using System.Net.WebSockets;
using TiffinMate.BLL.Interfaces.NotificationInterface;

using TiffinMate.BLL.Services.NotificationService;
using TiffinMate.BLL.Interfaces.OrderServiceInterface;
using TiffinMate.BLL.Services.OrderService;
using TiffinMate.DAL.Interfaces.OrderInterface;
using TiffinMate.DAL.Repositories.OrderRepository;

namespace TiffinMate.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            DotNetEnv.Env.Load();
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            var jwtRefreshKey = Environment.GetEnvironmentVariable("JWT_REFRESH_KEY");
            var env = Environment.GetEnvironmentVariable("IS_DEVELOPMENT");
            
            var defaultConnection = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");
            var hostUrl = Environment.GetEnvironmentVariable("HOST_URL");
            var hostApi = Environment.GetEnvironmentVariable("HOST_API");

            var brevoApiKey = Environment.GetEnvironmentVariable("BREVO_API_KEY");
            var brevoApiUrl = Environment.GetEnvironmentVariable("BREVO_API_URL");
            var brevoFromEmail = Environment.GetEnvironmentVariable("BREVO_FROM_EMAIL");
           
            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;                  
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<AppDbContext>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
            builder.Services.AddScoped<IFoodItemRepository, FoodItemRepository>();
            builder.Services.AddScoped<IFoodItemService, FoodItemService>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryServices>();
            builder.Services.AddScoped<IProviderService, ProviderService>();
            builder.Services.AddScoped<IProviderBrevoMailService, ProviderBrevoMailService>();
            builder.Services.AddScoped<IBrevoMailService, BrevoMailService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryServices>();
            builder.Services.AddScoped<IProviderVerificationService, ProviderVerificationService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<INotificationRepository,NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
           

      

            builder.Services.Configure<BrevoSettings>(options =>
            {
                options.ApiKey = brevoApiKey;
                options.ApiUrl = brevoApiUrl;
                options.FromEmail = brevoFromEmail;
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });


            builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(defaultConnection));

            builder.Services.AddScoped<Supabase.Client>(_ => new Supabase.Client(hostUrl, hostApi,
                new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true,
                }));

            builder.Services.AddAutoMapper(typeof(MappingProfile));
         
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer("AccessToken", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        })
        .AddJwtBearer("RefreshToken", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtRefreshKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false, 
                ValidateIssuerSigningKey = true
            };
        });

            builder.Services.AddSingleton<IOtpService>(provider =>
            {
                var accountSid = Environment.GetEnvironmentVariable("TWILIO_SID");
                var authToken = Environment.GetEnvironmentVariable("TWILIO_TOKEN");
                var verifySid = Environment.GetEnvironmentVariable("TWILIO_VERIFY_SID");

                return new OtpService(accountSid, authToken, verifySid);
            });

            var app = builder.Build();

          

            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2)
            });

            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2)
            });

            app.Map("/ws", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var socket = await context.WebSockets.AcceptWebSocketAsync();
                    var socketId = Guid.NewGuid().ToString();
                    WebSocketManager.AddSocket(socketId, socket);

                    try
                    {
                        var buffer = new byte[1024 * 4];
                        WebSocketReceiveResult result;
                        do
                        {
                            result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        } while (!result.CloseStatus.HasValue);

                        WebSocketManager.RemoveSocket(socketId);
                        await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        WebSocketManager.RemoveSocket(socketId);
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            });

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
            app.MapGet("/ping", () => "ping");
            app.Run();
        }
    }
}