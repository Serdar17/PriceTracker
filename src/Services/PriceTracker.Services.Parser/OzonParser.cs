using System.Text.RegularExpressions;
using OpenQA.Selenium;
using PriceTracker.Parser;
using PriceTracker.Services.Parser.Models;

namespace PriceTracker.Services.Parser;

public class OzonParser : IParser
{
    public async Task<ParseResult> ParseAsync(string url)
    {
        IWebDriver driver = DriverConfig.GetConfiguredWebDriver();
        driver.Navigate().GoToUrl(url);
        var title = driver.FindElement(By.XPath(".//div[@data-widget='webProductHeading']/h1")).Text;
        double price = 0;
        double priceCard = 0;
        try
        {
            var pattern = @"\d+,?\d+";
            var findPrice = driver.FindElement(By.XPath(".//div[@data-widget='webPrice']/div/div/div/div/span")).Text;
            findPrice = findPrice.Replace("\u2009", "");
            price = double.Parse(Regex.Match(findPrice, pattern).Value);
            var findCardPrice =
                driver.FindElement(By.XPath(".//span[@style='border-radius:8px;']/div/div/div/div/span")).Text;
            findCardPrice = findCardPrice.Replace("\u2009", "");
            priceCard = double.Parse(Regex.Match(findCardPrice, pattern).Value);
        }
        catch (NoSuchElementException)
        {
            var findPrice = driver.FindElement(By.XPath(".//h3[@class='_1He5n _36SPc _2kgEE _1KE2k fhbmm']/span[2]"));
            price = double.Parse(Regex.Replace(findPrice.Text, "\\s", ""));
        }
        finally
        {
            driver.Close();
        }
        return new ParseResult(title, price, priceCard);
    }
}