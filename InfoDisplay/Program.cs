using Gitec.InfoDisplay.Services;
using Gitec.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InfoDisplay.Data;
using Serilog;

namespace InfoDisplay;

public class Program
{
    public static void Main(string[] args)
    {
        ConfigurationService.Initialize("InfoDisplay");
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(ConfigurationService.GetLogFile(), rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
            .CreateLogger();

        Log.Information("Starting Info Display application...");

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(ConfigurationService.GetConnectionString()));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddRazorPages();

            builder.Services.AddHttpClient<RssFeedService>();

            builder.Services.AddScoped<InfoDisplayDbContext>();
            
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5184); // Default HTTP
                options.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps()); // Default HTTPS
            });
            


            var app = builder.Build();

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
            
        } catch (Exception ex)
        {
            Log.Fatal(ex, "An error occurred while starting the application.");
        } finally
        {
            Log.CloseAndFlush();
        }
    }
}