using PriceTracker.Services.Parser.Models;

namespace PriceTracker.Parser;

public interface IParser
{
    Task<ParseResult> ParseAsync(string url);
}