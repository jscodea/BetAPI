
using BetAPI.Data;
using BetAPI.Models;
using BetAPI.Services;
using Microsoft.EntityFrameworkCore;

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