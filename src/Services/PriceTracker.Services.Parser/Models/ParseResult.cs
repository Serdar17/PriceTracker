namespace PriceTracker.Services.Parser.Models;

public class ParseResult
{
    public string? Title { get; set; }
    public double? Price { get; set; }
    public double? CardPrice { get; set; }
    
    public string? Currency { get; set; }
    
    public ParseResult(string? title, double? price, double? cardPrice, string? currency = null)
    {
        Title = title;
        Price = price;
        CardPrice = cardPrice;
        Currency = currency;
    }

    public override string ToString()
    {
        return $"The product title is {Title}\n" +
               $"Price is {Price}\n" +
               $"Discounted price is {CardPrice}\n" +
               $"Currency is {Currency}";
    }
}