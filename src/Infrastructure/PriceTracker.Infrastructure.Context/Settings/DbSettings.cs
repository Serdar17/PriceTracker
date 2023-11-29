namespace PriceTracker.Infrastructure.Context.Settings;

public class DbSettings
{
    public const string SectionName = "Database";
    public string ConnectionString { get; private set; }
}