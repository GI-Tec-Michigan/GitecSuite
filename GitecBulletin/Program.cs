using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Services;
using Gitec.InfoDisplay.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;

namespace GitecBulletin;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services.AddOidcAuthentication(options =>
        {
            // Configure your authentication provider options here.
            // For more information, see https://aka.ms/blazor-standalone-auth
            builder.Configuration.Bind("Local", options.ProviderOptions);
        });

        ConfigurationService.Initialize("GitecBulletin");

        builder.Services.AddDbContext<BulletinDbContext>(options =>
        {
            options.UseSqlite(ConfigurationService.GetConnectionString());
        });

        builder.Services.AddScoped<ManagerService>();

        await builder.Build().RunAsync();
    }
}
