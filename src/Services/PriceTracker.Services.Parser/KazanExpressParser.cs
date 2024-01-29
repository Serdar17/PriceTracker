using Newtonsoft.Json.Linq;
using PriceTracker.Parser;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using PriceTracker.Services.Parser.Models;

namespace PriceTracker.Services.Parser
{
    public class KazanExpressParser : IParser
    {
        private static HttpClient HttpClient;

        public KazanExpressParser()
        {
            HttpClient = new()
            {
                BaseAddress = new Uri("https://api.kazanexpress.ru/api/v2/product/"),
            };

            //Без этого заголовка в запросе не подключается
            HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Chrome", "119.0.0.0"));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Host = "api.kazanexpress.ru";
            HttpClient.DefaultRequestHeaders.Referrer = new Uri("https://kazanexpress.ru/");
        }

        public async Task<ParseResult?> ParseAsync(string url)
        {
            var productParams = GetProductParams(url);
            var productIndex = productParams["productIndex"];

            using HttpResponseMessage response = await HttpClient.GetAsync(productIndex);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                JObject jObject = JObject.Parse(responseBody);
                JToken jTitle = jObject["payload"]["data"]["title"];
                JToken jSkuList = jObject["payload"]["data"]["skuList"];

                var skuList = jSkuList.ToObject<List<Dictionary<string, object>>>();
                string? title = jTitle.ToString();
                double? price;

                if (productParams["productId"] != null)
                {
                    var productId = int.Parse(productParams["productId"]);

                    //Цена товара, если выбраны дополнительные характеристики(цвет, размер)
                    price = skuList.Where(p => (int)(long)p["id"] == productId)
                        .Select(p => (double)p["purchasePrice"])
                        .First();
                }
                else
                    //Цена товара без выбранных характеристик
                    price = (double)skuList[0]["purchasePrice"];


                var parseResult = new ParseResult(title, price, cardPrice: null);
                return new ParseResult(title, price, cardPrice: 0.0);
            }
            else
                return null;
        }

        private static Dictionary<string, string?> GetProductParams(string url)
        {
            var productParams = new Dictionary<string, string?>();
            var productInfo = new Regex(@"(\d*)(\?\S*)?$").Match(url).Value;
            //Проверка для ссылок из мобильного приложения
            if (productInfo.Contains('&'))
                productInfo = productInfo.Split('&')[0];
            string productIndex;
            string? productId;

            /*Дополнительные параметры (skuId) в запросе появляются, если выбран определенный цвет/размер товара.
             В словарь добавляется постоянный индекс и Id (при наличии доп параметров) товара*/
            if (!productInfo.Contains('?'))
            {
                productIndex = productInfo;
                productId = null;
            }
            else
            {
                productIndex = new Regex(@"^(\d*)").Match(productInfo).Value;
                productId = new Regex(@"(\d*)$").Match(productInfo).Value;
            }

            productParams.Add("productIndex", productIndex);
            productParams.Add("productId", productId);

            return productParams;
        }
    }
}
