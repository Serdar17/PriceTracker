using System.Text.RegularExpressions;
using OpenQA.Selenium;
using PriceTracker.Parser;
using PriceTracker.Services.Parser.Models;

namespace PriceTracker.Services.Parser;

public class MegaMarketParser : IParser
{
    private const string Pattern = @"\d+,?\d+";
    
    public async Task<ParseResult> ParseAsync(string url)
    {
        var driver = DriverConfig.GetConfiguredWebDriver();
        double price = 0;
        double cardPrice = 0;
        var title = string.Empty;
        try
        {
            driver.Navigate().GoToUrl(url);
            title = driver.FindElement(By.XPath(".//h1[@itemprop='name']")).Text;
            var findPrice = driver.FindElements(By.XPath(".//del[@class='crossed-old-price-with-discount__crossed-old-price']")).First().Text;
            findPrice = findPrice.Replace(" ", "");
            price = double.Parse(Regex.Match(findPrice, Pattern).Value);
            var findCardPrice = driver.FindElements(By.XPath(".//span[@class='sales-block-offer-price__price-final']")).First().Text;
            findCardPrice = findCardPrice.Replace(" ", "");
            cardPrice = double.Parse(Regex.Match(findCardPrice, Pattern).Value);
        }
        catch (Exception e)
        {
            var findCardPrice =
                driver.FindElements(By.XPath(".//span[@class='sales-block-offer-price__price-final")).First().Text;
            findCardPrice = findCardPrice.Replace(" ", "");
            cardPrice = double.Parse(Regex.Match(findCardPrice, Pattern).Value);
            return new ParseResult(title, cardPrice, cardPrice);
        }
        finally
        {
            driver.Close();
        }

        return new ParseResult(title, price, cardPrice);
    }
}