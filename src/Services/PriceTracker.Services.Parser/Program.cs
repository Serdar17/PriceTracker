using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ParserTask
{
     class Program
    {
        static void Main(string[] args)
        {
            var urls = new List<string>();
            var ozon_url = @"https://www.ozon.ru/product/mikrofon-petlichnyy-noir-audio-nx-200-bodypack-chernyy-666162090/?from=share_android&utm_campaign=productpage_link&utm_medium=share_button&utm_source=smm";
            
            // urls.Add(@"https://ozon.ru/t/0lBDLQ4");
            urls.Add(ozon_url);
            urls.Add(@"https://ozon.ru/t/Ap9B358");
            // urls.Add(@"https://ozon.ru/t/EkDJaeE");
            // urls.Add(@"https://ozon.ru/t/L7P1Xzg");
            // urls.Add(@"https://ozon.ru/t/o3rGaMJ");
            urls.Add(@"https://ozon.ru/t/Q3X10Bz");
            // urls.Add(@"https://ozon.ru/t/V7NANpQ");
            
            // Wrapper(urls, "C:\\Test\\Test_ozon.txt", Parse);

            var yandex = @"https://market.yandex.ru/product--magnii-khelat-tab-150-ml-200-g-120-sht/268348034?sku=101308000511&do-waremd5=iw900iKoQT2v3eyYRP1aeg&uniqueId=843832";
            // var yand = @"https://market.yandex.ru/product--perchatki-muzhskie-kozhanye-zimnie-uteplennye/1918899606?sku=102311420204&do-waremd5=9Ug6OBLmyg04M_dRjHCfoA&uniqueId=1126241";
            var y = @"https://market.yandex.ru/product--sharf-crystel-eden-1286-4/1734729870?sku=101538922405&do-waremd5=WxpqmM5dgbkrRdvccwvE9g&uniqueId=1041197";
            var yandex_urls = new List<string>();
            yandex_urls.Add(yandex);
            // yandex_urls.Add(yand);
            yandex_urls.Add(y);

            Wrapper(yandex_urls, "C:\\Test\\Test_yandex.txt", Parse_YM);

        }

        //записываем в файл
        //сюда передаем ссылку на товар и мб метод функцию парсинга
        
        static void Wrapper(List<string> urls, string path, Func<string, Dictionary<string, string?>> parsing)
        {
            StreamWriter sw = new StreamWriter(path, true, Encoding.Default);
            foreach (var url in urls)
            {
                var info = parsing(url);
                foreach (var item in info)
                    sw.WriteLine($"{item.Key}: {item.Value}");
                sw.WriteLine("---------");
            }
            sw.Close();
        }

        public static Dictionary<string, string?> Parse(string url)
        {
            var commandTime = TimeSpan.FromSeconds(35);
            IWebDriver driver = new ChromeDriver(Environment.CurrentDirectory, new ChromeOptions(), commandTime);
            driver.Navigate().GoToUrl(url);
            var title = driver.FindElement(By.XPath(".//h1[@class='n4l']"));
            string price = null;
            string price_card = null;
            try
            {
                var find_price = driver.FindElement(By.XPath(".//span[@class='ln nl nl3']"));
                price = Regex.Replace(find_price.Text, "\\s", "");
                var find_card_price = driver.FindElement(By.XPath(".//span[@class='l5m ml3']"));
                price_card = Regex.Replace(find_card_price.Text, "\\s", "");
            }
            catch (NoSuchElementException)
            {
                var find_price = driver.FindElement(By.XPath(".//span[@class='ln nl ln4']"));
                price = Regex.Replace(find_price.Text, "\\s", "");
            }

            var dict = new Dictionary<string, string?>();
            dict.Add("title", title.Text);
            dict.Add("card_price", price_card);
            dict.Add("price", price);
            driver.Close();
            return dict;
        }

        public static Dictionary<string, string?> Parse_YM(string url)
        {
            var commandTime = TimeSpan.FromSeconds(150);
            IWebDriver driver = new ChromeDriver(Environment.CurrentDirectory, new ChromeOptions(), commandTime);
            driver.Navigate().GoToUrl(url);
            var title = driver.FindElement(By.XPath(".//h1[@class='_1a3VS D7c4V ZsaCc _2pkLA']"));
            string price = null;
            string price_card = null;
            try
            {
                var pattern = @":\d+";
                var find_price = driver.FindElement(By.XPath(".//span[@class='_8-sD9']"));
                Console.WriteLine(find_price.Text);
                price = Regex.Match(Regex.Replace(find_price.Text, "\\s", ""), pattern).Value.Replace(":", "");
                
                var find_card_price = driver.FindElement(By.XPath(".//div[@class='_2et7a _3i720 _3HQRD']/h3[@class='_1stjo']"));
                Console.WriteLine(find_card_price.Text);
                price_card = Regex.Replace(find_card_price.Text, @"[^\d]", "");
            }
            catch (NoSuchElementException)
            {
                var find_price = driver.FindElement(By.XPath(".//h3[@class='_1He5n _36SPc _2kgEE _1KE2k fhbmm']/span[2]"));
                Console.WriteLine(find_price.Text);
                price = Regex.Replace(find_price.Text, "\\s", "");
            }
                  
            var dict = new Dictionary<string, string?>();
            dict.Add("title", title.Text);
            dict.Add("card_price", price_card);
            dict.Add("price", price);
            driver.Close();
            return dict;
        }

    }
}