using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ParsingMarketPlaces;

public class ScrapingUnityAssetStore
{
    #region Variables

    private readonly int _pageStart = 1;
    private readonly int _pageEnd = int.MaxValue;
    private readonly string _search = "";
    private readonly bool _isSearch;
    private readonly bool _isPageStart;
    private readonly bool _isPageEnd;

    private int _page;
    private const string Url = "https://assetstore.unity.com";

    private string Link()
    {
        var link = Url;
        link += "/?";
        link += _isSearch ? $"q={_search}" : "";
        link += $"&orderBy={_page}";

        return link;
    }

    private List<string> _linksToAssets = new();

    private IWebDriver _driver = null;
    private FileInfo _file = new FileInfo(@"sample1.xlsx");
    private ExcelPackage _excelPackage = new ExcelPackage();

    #endregion

    #region Constructors

    public ScrapingUnityAssetStore(string search, int pageStart, int pageEnd)
    {
        this._search = search;
        this._pageStart = pageStart;
        this._pageEnd = pageEnd;

        _page = pageStart;

        _isSearch = true;
        _isPageStart = true;
        _isPageEnd = true;

        Console.WriteLine(Link());

        Initialize();
        GetReferencesToAssets();
        FillInExel();
        _excelPackage.SaveAs(_file);
        Clean();
    }

    public ScrapingUnityAssetStore(string search, int pageStart)
    {
        this._search = search;
        this._pageStart = pageStart;

        _page = pageStart;

        _isSearch = true;
        _isPageStart = true;

        Initialize();
        GetReferencesToAssets();
        FillInExel();
        _excelPackage.SaveAs(_file);
        Clean();
    }

    public ScrapingUnityAssetStore(string search)
    {
        this._search = search;

        _page = _pageStart;

        _isSearch = true;

        Initialize();
        GetReferencesToAssets();
        FillInExel();
        _excelPackage.SaveAs(_file);
        Clean();
    }

    public ScrapingUnityAssetStore()
    {
        _page = _pageStart;

        Initialize();
        GetReferencesToAssets();
        FillInExel();
        _excelPackage.SaveAs(_file);
        Clean();
    }

    #endregion

    #region Methods

    private void Initialize()
    {
        try
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("log-level=3");
            options.AddArgument("--ignore-certificate-errors-spki-list");
            options.AddArgument("--ignore-certificate-errors");
            // options.AddArguments("headless");

            Console.WriteLine("Initializing drivers");
            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(300);
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(Link());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _driver?.Quit();
            Initialize();
            throw;
        }
    }

    private void Clean()
    {
        Console.WriteLine("Clean");
        _driver.Close();
    }

    private void GetReferencesToAssets()
    {
        Console.WriteLine("GetHrefAssets");
        var elements = _driver.FindElements(By.ClassName(Element_ReferencesToAssets));
        for (int i = _pageStart; i < _pageEnd; i++)
        {
            if (_driver.Url != Link())
                _driver.Url = Link();
            foreach (var element in elements)
            {
                var elem = element.FindElement(By.TagName("a")).GetAttribute("href");
                if (elem != null)
                    _linksToAssets.Add(elem);
                Console.WriteLine($"GetHrefAsset completed {elem} ");
            }
        }
    }

    private void FillInExel()
    {
        Console.WriteLine("FillInExel");
        var excelPackage = new ExcelPackage(_file);
        excelPackage.Workbook.Worksheets.Add("DataBase Assets");
        var worksheet = excelPackage.Workbook.Worksheets[0];

        worksheet.Cells["A1"].Value = "Url";
        worksheet.Cells["B1"].Value = "Name";
        worksheet.Cells["C1"].Value = "Money";
        worksheet.Cells["D1"].Value = "Size files";
        worksheet.Cells["E1"].Value = "Description asset";

        for (int i = 0; i < 100; i++)
        {
            Console.WriteLine($"Scraping {i}\t{_linksToAssets[i]}");
            try
            {
                _driver.Url = _linksToAssets[i];
                var dataBase = Scraping(_driver);

                worksheet.Cells[$"A{i + 2}"].Value = $"{_driver.Url}";
                worksheet.Cells[$"B{i + 2}"].Value = $"{dataBase.Item1}";
                worksheet.Cells[$"C{i + 2}"].Value = $"{dataBase.Item2}";
                worksheet.Cells[$"D{i + 2}"].Value = $"{dataBase.Item3}";
                worksheet.Cells[$"E{i + 2}"].Value = $"{dataBase.Item4}";
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error {_driver.Url}");
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

    private static (string, string, string, string) Scraping(IWebDriver driver)
    {
        Console.WriteLine("Scraping");
        var elementAssetName = driver.FindElement(By.ClassName(Element_Packages_AssetName));
        var elementAssetMoney = driver.FindElement(By.ClassName(Element_Packages_AssetMoney));
        var elementAssetSize = driver.FindElement(By.ClassName(Element_Packages_AssetSizeFiles));
        var elementAssetDescription = driver.FindElement(By.ClassName(Element_Packages_AssetDescription));

        return (elementAssetName.Text, elementAssetMoney.Text, elementAssetSize.Text, elementAssetDescription.Text);
    }

    #endregion


    #region Elements

    private const string Element_ReferencesToAssets = "_138_e";

    private const string Element_Packages_AssetName = "cfm2v";
    private const string Element_Packages_AssetMoney = "mErEH"; //mErEH _223RA
    private const string Element_Packages_AssetSizeFiles = "_223RA";
    private const string Element_Packages_AssetDescription = "_1_3uP";

    #endregion
}