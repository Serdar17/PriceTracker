namespace PriceTracker.Services.Parser.Models;

public class ParseResult
{
    public string? Title { get; set; }
    public double? Price { get; set; }
    public double? CardPrice { get; set; }
    public ParseResult(string? title, double? price, double? cardPrice)
    {
        Title = title;
        Price = price;
        CardPrice = cardPrice;
    }
}