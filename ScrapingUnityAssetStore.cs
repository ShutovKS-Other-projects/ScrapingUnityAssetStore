using System.Collections.ObjectModel;
using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ParsingMarketPlaces;

public class ScrapingUnityAssetStore
{
    #region Variables

    private const string Url = "https://assetstore.unity.com";

    private readonly int _pageStart = 1;
    private readonly int _pageEnd = int.MaxValue;
    private readonly string _search = "";
    private readonly bool _isSearch;
    private readonly bool _isPageStart;
    private readonly bool _isPageEnd;

    private readonly int _page;

    private string Link()
    {
        var link = Url;
        link += "/?";
        link += _isSearch ? $"q={_search}" : "";
        link += $"&orderBy={_page}";

        return link;
    }

    private readonly List<string> _linksToAssets = new();

    private IWebDriver? _driver = null;
    private readonly FileInfo _file = new FileInfo(@"sample.xlsx");
    private readonly ExcelPackage _excelPackage = new ExcelPackage();

    #endregion

    #region Constructors

    public ScrapingUnityAssetStore(string search, int pageStart, int pageEnd)
    {
        _search = search;
        _pageStart = pageStart;
        _pageEnd = pageEnd;

        _page = pageStart;

        _isSearch = true;
        _isPageStart = true;
        _isPageEnd = true;

        Work();
    }

    public ScrapingUnityAssetStore(int pageStart, int pageEnd)
    {
        _pageStart = pageStart;
        _pageEnd = pageEnd;

        _page = pageStart;

        _isPageStart = true;
        _isPageEnd = true;

        Work();
    }

    public ScrapingUnityAssetStore(int pageStart)
    {
        _pageStart = pageStart;

        _page = pageStart;

        _isPageStart = true;

        Work();
    }

    public ScrapingUnityAssetStore(string search, int pageStart)
    {
        _search = search;
        _pageStart = pageStart;

        _page = pageStart;

        _isSearch = true;
        _isPageStart = true;

        Work();
    }

    public ScrapingUnityAssetStore(string search)
    {
        _search = search;

        _page = _pageStart;

        _isSearch = true;

        Work();
    }

    public ScrapingUnityAssetStore()
    {
        _page = _pageStart;

        Work();
    }

    #endregion

    #region Methods

    private void Work()
    {
        Console.WriteLine(Link());

        Initialize();
        GetReferencesToAssets();
        FillInExel();
        Clean();
    }

    private void Initialize()
    {
        try
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("log-level=3");
            options.AddArgument("--ignore-certificate-errors-spki-list");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArguments("headless");

            Console.WriteLine("Initializing drivers");
            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(300);
            _driver?.Manage().Window.Maximize();
            _driver?.Navigate().GoToUrl(Link());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _driver?.Quit();
            Initialize();
            throw;
        }
    }

    private void GetReferencesToAssets()
    {
        Console.WriteLine("GetHrefAssets");
        var elements = Element_ReferencesToAssets(_driver);
        for (var i = _pageStart; i < _pageEnd; i++)
        {
            if (_driver?.Url != Link())
                _driver!.Url = Link();

            if (elements == null) continue;
            foreach (var element in elements)
            {
                var elem = element.FindElement(By.TagName("a")).GetAttribute("href");
                if (elem != null)
                    _linksToAssets.Add(elem);
                Console.WriteLine($"GetHrefAsset completed {elem} ");
            }
        }

        Console.WriteLine($"GetHrefAssets: {_linksToAssets.Count}");
    }

    private void FillInExel()
    {
        Console.WriteLine("FillInExel");
        var excelPackage = new ExcelPackage(_file);
        if (excelPackage.Workbook.Worksheets.Count == 0)
            excelPackage.Workbook.Worksheets.Add("DataBase Assets");
        var worksheet = excelPackage.Workbook.Worksheets[0];

        worksheet.Cells[$"A1"].Value = "url";
        worksheet.Cells[$"B1"].Value = "name";
        worksheet.Cells[$"C1"].Value = "author";
        worksheet.Cells[$"D1"].Value = "grade";
        worksheet.Cells[$"E1"].Value = "favorite";
        worksheet.Cells[$"F1"].Value = "price";
        worksheet.Cells[$"G1"].Value = "license type";
        worksheet.Cells[$"H1"].Value = "file size";
        worksheet.Cells[$"I1"].Value = "latest release date";
        worksheet.Cells[$"J1"].Value = "description";

        for (var i = 0; i < _linksToAssets.Count; i++)
        {
            Console.WriteLine($"Scraping {i}\t{_linksToAssets[i]}");
            try
            {
                _driver!.Url = _linksToAssets[i];

                try
                {
                    worksheet.Cells[$"A{i + 2}"].Value = $"{_driver?.Url}";
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    worksheet.Cells[$"B{i + 2}"].Value = $"{Element_Packages_AssetName(_driver)?.Text}";
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    worksheet.Cells[$"C{i + 2}"].Value = $"{Element_Packages_AssetAuthor(_driver)?.Text}";
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    worksheet.Cells[$"D{i + 2}"].Value = $"{Element_Packages_AssetGrade(_driver)?.Text}";
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    worksheet.Cells[$"E{i + 2}"].Value = $"{Element_Packages_AssetFavorite(_driver)?.Text}";
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    worksheet.Cells[$"F{i + 2}"].Value = $"{Element_Packages_AssetPrice(_driver)?.Text}";
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    worksheet.Cells[$"G{i + 2}"].Value = $"{Element_Packages_AssetLicenseType(_driver)?.Text}";
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    worksheet.Cells[$"H{i + 2}"].Value = $"{Element_Packages_AssetFileSize(_driver)?.Text}";
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    worksheet.Cells[$"I{i + 2}"].Value = $"{Element_Packages_AssetLatestReleaseDate(_driver)?.Text}";
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    worksheet.Cells[$"J{i + 2}"].Value = $"{Element_Packages_AssetDescription(_driver)?.Text}";
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error {_driver!.Url}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                excelPackage.SaveAs(_file);
            }
        }
    }

    private void Clean()
    {
        Console.WriteLine("Clean");
        _driver?.Close();
    }

    #endregion


    #region Elements

    private static IWebElement? Element_ReferencesToAsset(ISearchContext? driver) =>
        driver?.FindElement(By.ClassName("_138_e"));

    private static ReadOnlyCollection<IWebElement>? Element_ReferencesToAssets(ISearchContext? driver) =>
        driver?.FindElements(By.ClassName("_138_e"));

    private static IWebElement? Element_Packages_AssetName(ISearchContext? driver) =>
        driver?.FindElement(By.ClassName("cfm2v"));

    private static IWebElement? Element_Packages_AssetAuthor(ISearchContext? driver) =>
        driver?.FindElement(By.ClassName("U9Sw1"));

    private static IWebElement? Element_Packages_AssetGrade(ISearchContext? driver) =>
        driver?.FindElement(By.ClassName("NoXio"));

    private static IWebElement? Element_Packages_AssetFavorite(ISearchContext? driver) =>
        driver?.FindElement(By.ClassName("ifont-favorite"));

    private static IWebElement? Element_Packages_AssetPrice(ISearchContext? driver) =>
        driver?.FindElement(By.ClassName("mErEH"));

    private static IWebElement? Element_Packages_AssetLicenseType(ISearchContext? driver) =>
        driver?.FindElement(By.ClassName("product-license")).FindElement(By.ClassName("SoNzt"));

    private static IWebElement? Element_Packages_AssetFileSize(ISearchContext? driver) =>
        driver?.FindElement(By.ClassName("product-size")).FindElement(By.ClassName("SoNzt"));

    private static IWebElement? Element_Packages_AssetLatestReleaseDate(ISearchContext? driver) =>
        driver?.FindElement(By.ClassName("product-date")).FindElement(By.ClassName("SoNzt"));

    private static IWebElement? Element_Packages_AssetDescription(ISearchContext? driver) =>
        driver?.FindElement(By.ClassName("_1_3uP"));

    #endregion
}