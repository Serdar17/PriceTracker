using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PriceTracker.Services.Parser;

public class DriverConfig
{
    public static IWebDriver GetConfiguredWebDriver()
    {
        var options = new ChromeOptions();
        // options.AddArgument("--no-sandbox");
        // options.AddArgument("--headless");
        // options.AddArgument("--window-size=1920,1080");
        // options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36");
        
        var chromeDriverService = ChromeDriverService.CreateDefaultService();
        chromeDriverService.HideCommandPromptWindow = true;
        var driver = new ChromeDriver(chromeDriverService, options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        return driver;
    }
}