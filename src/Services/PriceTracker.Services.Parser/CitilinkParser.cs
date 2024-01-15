using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PriceTracker.Parser;
using PriceTracker.Services.Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PriceTracker.Services.Parser
{
    public class CitilinkParser : IParser
    {
        public async Task<ParseResult> ParseAsync(string url)
        {
            var commandTime = TimeSpan.FromSeconds(150);
            var chromeOptions = new ChromeOptions();
            IWebDriver driver = new ChromeDriver(Environment.CurrentDirectory, new ChromeOptions(), commandTime);
            driver.Navigate().GoToUrl(url);
            var title = driver.FindElement(By.XPath(".//h1[@class='e1ubbx7u0 eml1k9j0 app-catalog-tn2wxd e1gjr6xo0']"));
            double price = 0;
            double price_card = 0;
            try
            {
                var find_price = driver.FindElement(By.XPath(".//div[@data-meta-name='PriceBlock__price']/span"));
                price = double.Parse(Regex.Replace(find_price.Text, @"[^\d]", ""));
                var find_card_price = driver.FindElement(By.XPath(".//div[@data-meta-name='PriceBlock__club-price']/span[1]/span[1]"));
                price_card = double.Parse(Regex.Replace(find_card_price.Text, @"[^\d]", ""));
            }
            catch (NoSuchElementException e)
            {
                var find_price = driver.FindElement(By.XPath(".//div[@data-meta-name='PriceBlock__price']/span"));
                price = double.Parse(Regex.Replace(find_price.Text, @"[^\d]", ""));
            }
            catch (Exception ex)
            {
                price = -1;
            }

            return new ParseResult(title.Text, price, price_card);
        }
    }
}
