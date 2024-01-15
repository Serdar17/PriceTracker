using System.Globalization;
using OpenQA.Selenium;
using PriceTracker.Parser;
using PriceTracker.Services.Parser.Models;

namespace PriceTracker.Services.Parser;

public class TestParser : IParser
{
    private readonly IWebDriver _driver;

    public TestParser()
    {
        _driver = DriverConfig.GetConfiguredWebDriver();
    }

    public async Task<ParseResult> ParseAsync(string url)
    {
        _driver.Navigate().GoToUrl(url);
        var title = _driver.FindElements(By.XPath(".//div[@class='esh-catalog-name ml-3']/span")).First().Text;
        var price = _driver.FindElements(By.XPath(".//div[@class='esh-catalog-price mr-3']/span")).First().Text;
        Console.WriteLine(title);
        return new ParseResult(title, double.Parse(price, CultureInfo.InvariantCulture), double.Parse(price, CultureInfo.InvariantCulture));
    }
}