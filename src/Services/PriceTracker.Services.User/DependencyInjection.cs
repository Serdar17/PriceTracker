using Microsoft.Extensions.DependencyInjection;

namespace PriceTracker.Services.User;

public static class DependencyInjection
{
    public static IServiceCollection AddAppUserServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
        
        return services;
    }
}