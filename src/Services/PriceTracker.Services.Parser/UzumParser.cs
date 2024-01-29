using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using PriceTracker.Parser;
using PriceTracker.Services.Parser.Models;

namespace PriceTracker.Services.Parser;

public class UzumParser : IParser
{
    private readonly ILogger<UzumParser> _logger;

    public UzumParser(ILogger<UzumParser> logger)
    {
        _logger = logger;
    }

    public async Task<ParseResult?> ParseAsync(string url)
    {
        _logger.LogInformation("Starting {name} parsing", nameof(UzumParser));
        var driver = DriverConfig.GetConfiguredWebDriver();
        string title, currency;
        double price = 0.0, cardPrice = 0.0;

        try
        {
            driver.Navigate().GoToUrl(url);
            driver.Navigate().Refresh();
            title = driver.FindElement(By.XPath(".//h1[@class='title']")).Text;
        
            var findCardPrice = driver
                .FindElement(By.XPath(".//div[@data-test-id='text__product-price']/span[@class='currency']")).Text;
            cardPrice = GetPriceWithoutCurrency(findCardPrice);
            Console.WriteLine(findCardPrice);
            var findPrice = driver
                .FindElement(By.XPath(".//div[@class='block-part-content']/div/span[@class='currency old-price']"))
                .Text;
            price = GetPriceWithoutCurrency(findPrice);
            Console.WriteLine(price);
            currency = findCardPrice.Split().Last();
        }
        catch (Exception ex)
        {
            _logger.LogError("Uzum market parsing error occurred. Error message: {Message}", ex.Message);
            return null;
        }
        finally
        {
            driver.Close();
        }

        var result = new ParseResult(title, price, cardPrice, currency);
        _logger.LogInformation("Uzum market parsing was successful. The parsed obj: {ParseResult}", result);

        return result;
    }

    private double GetPriceWithoutCurrency(string price)
    {
        return double.Parse(string.Join(string.Empty, price.Split()[..^1]));
    }
}