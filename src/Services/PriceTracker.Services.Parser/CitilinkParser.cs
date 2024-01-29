using OpenQA.Selenium;
using PriceTracker.Parser;
using PriceTracker.Services.Parser.Models;
using System.Text.RegularExpressions;

namespace PriceTracker.Services.Parser
{
    public class CitilinkParser : IParser
    {
        public async Task<ParseResult?> ParseAsync(string url)
        {
            IWebDriver driver = DriverConfig.GetConfiguredWebDriver();
            var title = string.Empty;
            double price = 0;
            double priceCard = 0;
            try
            {
                driver.Navigate().GoToUrl(url);
                title = driver.FindElement(By.XPath(".//h1[@class='e1ubbx7u0 eml1k9j0 app-catalog-tn2wxd e1gjr6xo0']"))
                    .Text;
                var findPrice = driver.FindElement(By.XPath(".//div[@data-meta-name='PriceBlock__price']/span"));
                price = double.Parse(Regex.Replace(findPrice.Text, @"[^\d]", ""));
                var findCardPrice =
                    driver.FindElement(By.XPath(".//div[@data-meta-name='PriceBlock__club-price']/span[1]/span[1]"));
                priceCard = double.Parse(Regex.Replace(findCardPrice.Text, @"[^\d]", ""));
            }
            catch (NoSuchElementException e)
            {
                var findPrice = driver.FindElement(By.XPath(".//div[@data-meta-name='PriceBlock__price']/span"));
                price = double.Parse(Regex.Replace(findPrice.Text, @"[^\d]", ""));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                driver.Close();
            }

            return new ParseResult(title, price, priceCard);
        }
    }
}
