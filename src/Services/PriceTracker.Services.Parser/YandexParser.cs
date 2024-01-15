using OpenQA.Selenium;
using PriceTracker.Parser;
using System.Text.RegularExpressions;
using PriceTracker.Services.Parser.Models;

namespace PriceTracker.Services.Parser
{
    public class YandexParser : IParser
    {
        public async Task<ParseResult> ParseAsync(string url)
        {
            IWebDriver driver = DriverConfig.GetConfiguredWebDriver();
            ParseResult parseResult;
            try
            {
                driver.Navigate().GoToUrl(url);
                var findTitle =  driver.FindElement(By.XPath(".//h1[@data-additional-zone='title']"));
                var title = findTitle.Text;
                double price = 0;
                double priceCard = 0;
                try
                {
                    var pattern = @":\d+";
                    var findPrice = driver.FindElement(By.XPath(".//h3[@data-auto='snippet-price-current']"));
                    Console.WriteLine(findPrice.Text);
                    price = double.Parse(Regex.Match(Regex.Replace(findPrice.Text, "\\s", ""), pattern).Value.Replace(":", ""));
                    var findCardPrice = driver.FindElement(By.XPath(".//h3[@data-auto='snippet-price-current']"));
                    Console.WriteLine(findCardPrice.Text);
                    priceCard = double.Parse((Regex.Replace(findCardPrice.Text, @"[^\d]", "")));
                }
                catch (NoSuchElementException)
                {
                    var findPrice = driver.FindElement(By.XPath(".//h3[@class='_1He5n _36SPc _2kgEE _1KE2k fhbmm']/span[2]"));
                    Console.WriteLine(findPrice.Text);
                    price = double.Parse(Regex.Replace(findPrice.Text, "\\s", ""));
                }
                catch (Exception ex)
                {
                    price = -1;
                }
                parseResult = new ParseResult(title, price, priceCard);
            }
            catch (Exception e)
            {
                return new ParseResult(null, null, null);
            }
            finally
            {
                driver?.Close();
            }
            return parseResult;
        }
    }
}