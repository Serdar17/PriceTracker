using System.Text.RegularExpressions;
using OpenQA.Selenium;
using PriceTracker.Parser;
using PriceTracker.Services.Parser.Models;

namespace PriceTracker.Services.Parser;

public class MegaMarketParser : IParser
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
            driver.Navigate().Refresh(); // при первой загрузке страницы показывает не ту цену, после перезагрузки цена стает актуальной
            title = driver.FindElement(By.XPath(".//h1[@itemprop='name']")).Text;
            var findPrice = driver.FindElement(By.XPath(".//div[@class='sales-block-offer-price sales-block-offer-price_active']//del[@class='crossed-old-price-with-discount__crossed-old-price']")).Text;
            findPrice = findPrice.Replace(" ", "");
            price = double.Parse(Regex.Match(findPrice, Pattern).Value);
            var findCardPrice = driver.FindElement(By.XPath(".//div[@class='sales-block-offer-price sales-block-offer-price_active']/div/span[@class='sales-block-offer-price__price-final']")).Text;
            findCardPrice = findCardPrice.Replace(" ", "");
            cardPrice = double.Parse(Regex.Match(findCardPrice, Pattern).Value);
        }
        catch (Exception e)
        {
            var findCardPrice =
                driver.FindElements(By.XPath(".//div[@class='sales-block-offer-price sales-block-offer-price_active']/div/span[@class='sales-block-offer-price__price-final']")).First().Text;
            findCardPrice = findCardPrice.Replace(" ", "");
            cardPrice = double.Parse(Regex.Match(findCardPrice, Pattern).Value);
            return new ParseResult(title, cardPrice, cardPrice);
        }
        finally
        {
            // driver.Close();
        }
        
        if (string.IsNullOrEmpty(title))
        {
            return null;
        }

        return new ParseResult(title, price, cardPrice);
    }
}