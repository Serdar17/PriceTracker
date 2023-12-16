// using System.Text.RegularExpressions;
// using OpenQA.Selenium;
// using OpenQA.Selenium.Chrome;
// using PriceTracker.Parser;
// using PriceTracker.Services.Parser.Models;
//
// namespace PriceTracker.Services.Parser;
//
// public class OzonParser : IParser
// {
//     public async Task<ParseResult> ParseAsync(string url)
//     {
//         var commandTime = TimeSpan.FromSeconds(30);
//         var chromeDirectory = @$"{Environment.CurrentDirectory}/bin/Debug/net7.0" ;
//         IWebDriver driver = new ChromeDriver(chromeDirectory, new ChromeOptions(), commandTime);
//         driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
//         driver.Navigate().GoToUrl(url);
//         var title = driver.FindElement(By.XPath(".//div[@data-widget='webProductHeading']/h1")).Text;
//         double price = 0;
//         double priceCard = 0;
//         try
//         {
//             // var pattern = @":\d+";
//             var pattern = @"\d+\,?\d+";
//             var findPrice = driver.FindElement(By.XPath(".//div[@data-widget='webPrice']/div/div/div/div/span"));
//             Console.WriteLine(findPrice.Text);
//             price = double.Parse(Regex.Replace(findPrice.Text, pattern, ""));
//             var findCardPrice = driver.FindElement(By.XPath(".//span[@style='border-radius:8px;']/div/div/div/div/span"));
//             Console.WriteLine(findCardPrice.Text);
//             priceCard = double.Parse(Regex.Replace(findCardPrice.Text, pattern, ""));
//         }
//         catch (NoSuchElementException)
//         {
//             var findPrice = driver.FindElement(By.XPath(".//h3[@class='_1He5n _36SPc _2kgEE _1KE2k fhbmm']/span[2]"));
//             Console.WriteLine(findPrice.Text);
//             price = double.Parse(Regex.Replace(findPrice.Text, "\\s", ""));
//         }
//         // catch (Exception ex)
//         // {
//         //     price = -1;
//         // }
//         driver.Close();
//         return new ParseResult(title, price, priceCard);
//     }
// }