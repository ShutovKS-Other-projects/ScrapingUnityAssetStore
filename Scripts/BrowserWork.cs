using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;

namespace ParsingMarketPlaces;

public class BrowserWork
{
    public IWebDriver? Driver { get; private set; }
    private readonly bool _isVisible = false;

    public BrowserWork(TypeBrowser typeBrowser = TypeBrowser.Chrome, bool isVisible = false)
    {
        try
        {
            var options = new ChromeOptions();
            options.AddArguments(
                "log-level=3",
                "--ignore-certificate-errors-spki-list",
                "--ignore-certificate-errors",
                _isVisible ? "" : "headless");

            Driver = new ChromeDriver(options);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(300);
            if (_isVisible)
                Driver?.Manage().Window.Maximize();
        }
        catch (Exception e)
        {
            Console.WriteLine("Open browser failed");
            throw;
        }
    }
}

public enum TypeBrowser
{
    Chrome,
    Chromium,
    Edge,
    Firefox,
    InternetExplorer,
    Safari
}