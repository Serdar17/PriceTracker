using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Infrastructure.Context.Factories;
using PriceTracker.Infrastructure.Context.Settings;

namespace PriceTracker.Infrastructure.Context;

public static class DependencyInjection
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = PriceTracker.Settings.Settings.Load<DbSettings>(DbSettings.SectionName);
        services.AddSingleton(settings);

        var dbInitOptionsDelegate = DbContextOptionsFactory.Configure(settings.ConnectionString);
        services.AddDbContextFactory<AppDbContext>(dbInitOptionsDelegate);
        
        
        return services;
    }
}