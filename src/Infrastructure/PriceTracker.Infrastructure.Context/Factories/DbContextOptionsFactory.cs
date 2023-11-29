using Microsoft.EntityFrameworkCore;

namespace PriceTracker.Infrastructure.Context.Factories;

public static class DbContextOptionsFactory
{
    private const string MigrationProjectPrefix = "PriceTracker.Infrastructure.Context";

    public static DbContextOptions<AppDbContext> Create(string connectionString)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();

        Configure(connectionString).Invoke(builder);

        return builder.Options;
    }

    public static Action<DbContextOptionsBuilder> Configure(string connectionString)
    {
        return (builder) =>
        {
            builder.UseNpgsql(connectionString,
                opt => opt

                    .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                    .MigrationsHistoryTable("_EFMigrationsHistory", "public")
                    .MigrationsAssembly(MigrationProjectPrefix)
            ).UseSnakeCaseNamingConvention();

            builder.EnableSensitiveDataLogging();
            builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        };
    }
}