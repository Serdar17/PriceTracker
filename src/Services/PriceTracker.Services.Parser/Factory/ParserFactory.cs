using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Common.Constants;
using PriceTracker.Parser;

namespace PriceTracker.Services.Parser.Factory;

public class ParserFactory : IParserFactory
{
    private readonly IEnumerable<IParser> _parsers;

    public ParserFactory(IServiceProvider provider)
    {
        _parsers = provider.CreateScope().ServiceProvider.GetServices<IParser>();
    }

    public IParser? CreateParser(string parser)
    {
        switch (parser)
        {
            case MarketPlace.YandexMarket:
                return _parsers.First(x => x is YandexParser);
            case MarketPlace.KazanExpress:
                return _parsers.First(x => x is KazanExpressParser);
            // case MarketPlace.Ozon:
            //     return _parsers.First(x => x is OzonParser);
            case MarketPlace.MVideo:
                return _parsers.First(x => x is MVideoParser);
            case MarketPlace.Test:
                return _parsers.First(x => x is TestParser);
            default:
                return null;
        };
    }
}