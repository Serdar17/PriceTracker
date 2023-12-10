using PriceTracker.Parser.Models;

namespace PriceTracker.Parser;

public interface IParser
{
    Task<ParseResult> ParseAsync();
}