using PriceTracker.Parser;

namespace PriceTracker.Services.Parser.Factory;

public interface IParserFactory
{
    IParser? CreateParser(string parser);
}