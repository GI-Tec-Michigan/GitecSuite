using Gitec.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GTBulletin.Data;
using Serilog;

namespace GTBulletin;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(@"C:\ProgramData\Gitec\Logs\events.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
            .CreateLogger();

        Log.Information("Starting GTBulletin application...");

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            ConfigurationService.Init("GTBulletin");
            
            builder.Services.AddDbContext<InfoBoardDbContext>(options =>
                options.UseSqlite(ConfigurationService.GetDatabasePath()));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            
            builder.Services.AddRazorPages();

            var app = builder.Build();
            
            // **Ensure Database is Created and Migrations are Applied**
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<InfoBoardDbContext>();
                    dbContext.Database.Migrate(); // Apply migrations automatically
                    Log.Information("Database has been ensured and migrations applied.");
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An error occurred while applying migrations.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
                .WithStaticAssets();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An unhandled exception occurred during startup.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}