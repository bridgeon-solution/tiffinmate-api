
using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Supabase;
using TiffinMate.BLL.Interfaces.ProviderServiceInterafce;
using TiffinMate.BLL.Mapper;
using TiffinMate.BLL.Services;
using TiffinMate.BLL.Services.AWS;
using TiffinMate.BLL.Services.ProviderServices;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities.AWS;
using TiffinMate.DAL.Interfaces.ProviderInterface;
using TiffinMate.DAL.Repositories.ProviderRepositories;

namespace TiffinMate.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


           /* AwsConfig.BucketName = builder.Configuration["AWS:BucketName"];*/

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
            // AWS Configuration
            builder.Services.AddDefaultAWSOptions(new AWSOptions
            {
                Region = Amazon.RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"]),
                Credentials = new Amazon.Runtime.BasicAWSCredentials(
                    builder.Configuration["AWS:AccessKey"],
                    builder.Configuration["AWS:SecretKey"])
            });
            builder.Services.AddAWSService<IAmazonS3>();

            builder.Services.AddScoped<IProviderService, ProviderService>();
            // Supabase configuration
            builder.Services.AddScoped<Supabase.Client>(_ =>
                new Supabase.Client(builder.Configuration.GetConnectionString("HostUrl"),
                                    builder.Configuration.GetConnectionString("HostAPI"),
                                    new SupabaseOptions
                                    {
                                        AutoRefreshToken = true,
                                        AutoConnectRealtime = true,
                                    }));

            // Database Context
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Custom services
            builder.Services.AddScoped<IProviderRepository, ProviderRepository>();

            // AWSProviderService registration with bucket name
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowReactApp");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
