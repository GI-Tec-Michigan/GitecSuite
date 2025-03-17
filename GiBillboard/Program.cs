using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Services;
using Gitec.Services;
using Microsoft.EntityFrameworkCore;

namespace GiBillboard;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();

        ConfigurationService configurationService = new ConfigurationService();
        configurationService.Initialize(AppConstants.AppName);

        // Add services for static assets
        builder.Services.AddScoped<SchedulePackageService>();
        builder.Services.AddScoped<DisplayThemeService>();
        builder.Services.AddScoped<DisplayScreenService>();
        builder.Services.AddScoped<BoardService>();
        builder.Services.AddScoped<ElementService>();
        builder.Services.AddScoped<DataSeedService>(); // Ensure it's scoped

        // Add DbContext and DataSeedService
        builder.Services.AddDbContext<GitecBulletinDbContext>(options =>
            options.UseSqlite(configurationService.GetConnectionString()));

        var app = builder.Build();

        // Properly resolve and execute DataSeedService within a scope
        using (var scope = app.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var seed = scopedServices.GetRequiredService<DataSeedService>();
            seed.SeedData(false);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages().WithStaticAssets();

        app.Run();
    }
}