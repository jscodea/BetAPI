
using BetAPI.Data;
using BetAPI.Models;
using BetAPI.Services;
using BetAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BetAPI.Handlers;
using System.Text;

namespace BetAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<BetAPIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("BetAPIContext")));

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IAppCache, AppCache>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IEventService, EventService>();
            builder.Services.AddTransient<IBetService, BetService>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IBetRepository, BetRepository>();
            builder.Services.AddTransient<IEventRepository, EventRepository>();
            builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
            builder.Services.AddHostedService<ConsumeScopedServiceHostedService>();
            builder.Services.AddScoped<IBackgroundConsumerService, BackgroundResultsConsumerService>();
            builder.Services.AddAuthentication(i =>
            {
                i.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                i.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                i.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireSignedTokens = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = null
                };
                options.SecurityTokenValidators.Clear();
                options.SecurityTokenValidators.Add(new DBKeyJWTValidationHandler(builder.Configuration.GetValue<string>("JwtSalt")));
                /*options.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        return Task.FromResult(0);
                    }
                };*/
            });

            var logger = new LoggerConfiguration()
              .ReadFrom.Configuration(builder.Configuration)
              .Enrich.FromLogContext()
              .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                SeedData.Initialize(services, builder.Configuration.GetValue<bool>("FreshSeed"));
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.MapControllers();

            app.Run();
        }
    }
}