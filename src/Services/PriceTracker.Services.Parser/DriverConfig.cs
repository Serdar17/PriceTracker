using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PriceTracker.Services.Parser;

public class DriverConfig
{
    private static IWebDriver? _driver;
    
    public static IWebDriver GetConfiguredWebDriver()
    {
        // if (_driver is not null)
        //     return _driver;
        
        var commandTimeout = TimeSpan.FromSeconds(30);
        var options = new ChromeOptions();
        options.AddArgument("--no-sandbox");
        options.AddArgument("--headless");
        options.AddArgument("--window-size=1920,1080");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36");
        
        var chromeDriverService = ChromeDriverService.CreateDefaultService();
        chromeDriverService.HideCommandPromptWindow = true;
        var driver = new ChromeDriver(chromeDriverService, options, commandTimeout);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        return driver;
    }
}