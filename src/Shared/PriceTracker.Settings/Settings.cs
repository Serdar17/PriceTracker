using Microsoft.Extensions.Configuration;

namespace PriceTracker.Settings;

public abstract class Settings
{
    public static T Load<T>(string key, IConfiguration? configuration = null)
    {
        var settings = Activator.CreateInstance<T>();

        SettingsFactory.Create(configuration).GetSection(key).Bind(settings, (x) => { x.BindNonPublicProperties = true; });

        return settings;
    }
}