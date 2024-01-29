using System.Text.RegularExpressions;
using OpenQA.Selenium;
using PriceTracker.Parser;
using PriceTracker.Services.Parser.Models;

namespace PriceTracker.Services.Parser;

public class MVideoParser : IParser
{
    private const string Pattern = @"\d+,?\d+";
    
    public async Task<ParseResult?> ParseAsync(string url)
    {
        var driver = DriverConfig.GetConfiguredWebDriver();
        double price = 0;
        double cardPrice = 0;
        var title = string.Empty;
        try
        {
            driver.Navigate().GoToUrl(url);
            title = driver.FindElement(By.XPath(".//h1[@itemprop='name']")).Text;
            var findPrice = driver.FindElement(By.XPath(".//div[@class='price price--pdp-emphasized-personal-price ng-star-inserted']/span[@class='price__sale-value ng-star-inserted']")).Text;
            findPrice = findPrice.Replace(" ", "");
            price = double.Parse(Regex.Match(findPrice, Pattern).Value);
            var findCardPrice = driver.FindElement(By.XPath(".//div[@class='price price--pdp-emphasized-personal-price ng-star-inserted']/span[@class='price__main-value']")).Text;
            findCardPrice = findCardPrice.Replace(" ", "");
            cardPrice = double.Parse(Regex.Match(findCardPrice, Pattern).Value);
        }
        catch (Exception e)
        {
            var findCardPrice =
                driver.FindElement(By.XPath(".//div[@class='price price--pdp-emphasized-personal-price ng-star-inserted']/span[@class='price__main-value']")).Text;
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