namespace PriceTracker.Common.Constants;

public static class MarketPlace
{
    public const string UzumMarket = "UzumMarket";
    public const string YandexMarket = "Yandex Market";
    public const string KazanExpress = "KazanExpress";
    public const string Ozon = "Ozon";
    public const string MVideo = "MVideo";
    public const string MegaMarket = "MegaMarket";
    public const string Citilink = "Citilink";
    public const string Localhost = "Localhost";

    public static List<string> GetAvailableMarketPlaces =>
        new()
        {
            UzumMarket,
            // YandexMarket,
            KazanExpress,
            MVideo,
            MegaMarket,
            Citilink,
            Localhost,
        };
}