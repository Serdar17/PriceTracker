// namespace PriceTracker.Common.Helpers;
//
// public static class MessageHelper
// {
//     public static string GetNotificationMessage(Product product, Price price, ParseResult result)
//     {
//         var currency = result.Currency ?? "\u20bd";
//         var message = $"\ud83d\udd14 Уведомление об изменении цены!\n" +
//                       $"\ud83d\udcb0 Прошлая цена без скидки: {price.CurrentPrice} {currency} \n" +
//                       $"\ud83d\udcb3 Прошлая цена по скидке/карте: {price.DiscountedPrice} {currency} \n" +
//                       $"\n" +
//                       $"\ud83d\udcbc Новая цена без скидки: *{result.Price}* {currency} \n" +
//                       $"\ud83d\udcb3 Новая цена по скидке/карте: *{result.CardPrice}* {currency} \n" +
//                       $"\n" +
//                       $"\ud83d\udd17 [Ссылка на товар]({product.Link})";
//         return message;
//     }
// }