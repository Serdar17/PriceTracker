using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PriceTracker.Infrastructure.Context.Setup;

public static class DbInitializer
{
    public static void Execute(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        ArgumentNullException.ThrowIfNull(scope);
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        using var context = dbContextFactory.CreateDbContext();
        context.Database.Migrate();
    }
}