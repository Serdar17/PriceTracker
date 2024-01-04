namespace PriceTracker.Common.Constants;

public static class MarketPlace
{
    public const string YandexMarket = "Yandex Market";
    public const string KazanExpress = "Kazan Express";
    public const string Ozon = "Ozon";
    public const string Test = "test";

    public static List<string> GetAvailableMarketPlaces =>
        new()
        {
            YandexMarket,
            KazanExpress,
            Ozon,
            Test,
        };
    
    // public static readonly List<string> AvailableMarketPlaces = new()
    // {
    //     // "Ozon",
    //     "Yandex Market",
    //     "Kazan Express",
    // };
}