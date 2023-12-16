using OpenQA.Selenium.Chrome;
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
            var commandTime = TimeSpan.FromSeconds(150);
            var chromeDirectory = @$"{Environment.CurrentDirectory}/bin/Debug/net7.0" ;
            IWebDriver driver = new ChromeDriver(chromeDirectory, new ChromeOptions(), commandTime);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            driver.Navigate().GoToUrl(url);
            var find_title = driver.FindElement(By.XPath(".//h1[@class='_1a3VS D7c4V ZsaCc _2pkLA']"));
            var title = find_title.Text;
            double price = 0;
            double price_card = 0;
            try
            {
                var pattern = @":\d+";
                var find_price = driver.FindElement(By.XPath(".//span[@class='_8-sD9']"));
                Console.WriteLine(find_price.Text);
                price = double.Parse(Regex.Match(Regex.Replace(find_price.Text, "\\s", ""), pattern).Value.Replace(":", ""));

                var find_card_price = driver.FindElement(By.XPath(".//div[@class='_2et7a _3i720 _3HQRD']/h3[@class='_1stjo']"));
                Console.WriteLine(find_card_price.Text);
                price_card = double.Parse((Regex.Replace(find_card_price.Text, @"[^\d]", "")));
            }
            catch (NoSuchElementException)
            {
                var find_price = driver.FindElement(By.XPath(".//h3[@class='_1He5n _36SPc _2kgEE _1KE2k fhbmm']/span[2]"));
                Console.WriteLine(find_price.Text);
                price = double.Parse(Regex.Replace(find_price.Text, "\\s", ""));
            }
            catch (Exception ex)
            {
                price = -1;
            }
            driver.Close();
            return new ParseResult(title, price, price_card);
        }
    }

}
