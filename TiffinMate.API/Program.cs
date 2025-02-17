
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
using TiffinMate.BLL.Hubs;
using TiffinMate.BLL.Interfaces;
using TiffinMate.BLL.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Quartz;
using TiffinMate.BLL.Jobs;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using TiffinMate.API.Controllers;
using TiffinMate.BLL.Services.GoogleAuthService;
using Microsoft.AspNetCore.SignalR;
using TiffinMate.BLL.Custom;


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

            var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
            var googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
            var callbackpath = Environment.GetEnvironmentVariable("CALLBACK_PATH");

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddSignalR();
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
            builder.Services.AddScoped<IRatingService, RatingService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IReviewRaingRepository, ReviewRatingRepository>();
            builder.Services.AddScoped<INotificationRepository,NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            builder.Services.AddScoped<RefreshInterface, refreshService>();
            builder.Services.AddScoped<IGoogleAuth, GoogleauthService>();

            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                var jobKey = new JobKey("billing-job", "billing");
                q.AddJob<BillingJob>(opts => opts.WithIdentity(jobKey));
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("billing-trigger", "billing")
                    .WithCronSchedule("0 0 1 1 * ?"));
            });

            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            builder.Services.AddScoped<IBillingService, BillingService>();
            builder.Services.Configure<BrevoSettings>(options =>
            {
                options.ApiKey = brevoApiKey;
                options.ApiUrl = brevoApiUrl;
                options.FromEmail = brevoFromEmail;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                    builder
                        .WithOrigins("http://localhost:5175", "http://localhost:5174", "http://localhost:5180", "https://beta.tiffinmate.online", "https://betaprovider.tiffinmate.online", "https://betaadmin.tiffinmate.online", "https://tiffinmate.online", "https://admin.tiffinmate.online", "https://provider.tiffinmate.online") 

                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });


            builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(defaultConnection, npgsqlOptions => npgsqlOptions.CommandTimeout(60)
));

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

            builder.Services.AddAuthentication(options =>
            {
                // Use cookies for authentication
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // Use GoogleDefaults for consistency
            })
  .AddCookie(options =>
  {
      options.LoginPath = "/google"; // Path to redirect for login
      options.LogoutPath = "/login"; // Path to redirect for logout
      options.AccessDeniedPath = "/login"; // Path for access denied
      options.ExpireTimeSpan = TimeSpan.FromMinutes(15); // Adjusted expiration time
      options.SlidingExpiration = true; // Enable sliding expiration
  })

  .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
  {
      options.ClientId = googleClientId;
      options.ClientSecret = googleClientSecret;
      options.CallbackPath = callbackpath; // Should match the registered callback in Google Console

      // Add required scopes
      options.Scope.Add("email");
      options.Scope.Add("profile");

      // Retrieve the user's name and email claims
      options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
      options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
  });

            var app = builder.Build();
            
          

           
            if (env == "Development")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHttpsRedirection();
            }
            app.UseCors("AllowSpecificOrigin");
            //app.UseMiddleware<LoggingMiddleware>();          
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHub<NotificationHub>("/adminHub");

            app.MapGet("/ping", () => "ping");
            app.Run();
        }
    }
}