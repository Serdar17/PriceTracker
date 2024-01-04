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
        var title = _driver.FindElement(By.XPath(".//div[@class='_title_1sfv9_20']/h2[1]")).Text;
        var price = _driver.FindElement(By.XPath(".//div[@class='_title_1sfv9_20']/h2[2]")).Text;
        Console.WriteLine(title);
        return new ParseResult(title, double.Parse(price), double.Parse(price));
    }
}