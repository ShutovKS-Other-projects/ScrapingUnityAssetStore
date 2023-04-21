using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OfficeOpenXml;
using System.IO;

#region Variables

IWebDriver driver = null;

var url = "https://assetstore.unity.com";
var search = "Dungeon";
var page = 0;
string Link() => $"{url}?q={search}&orderBy={page}";

var linksToAssets = new List<string>();
var linksToAssetsExel = new List<string>();

#endregion

#region Program

Initialize();

Console.WriteLine("Search linksToAssets");
for (; linksToAssets.Count < 100;)
{
    GetHrefAsset(driver, ref linksToAssets);
    page++;
    if (driver.Url != Link())
        driver.Url = Link();
}

Console.WriteLine("Fill out Exel");
var file = new FileInfo(@"D:\Work\Вуз\Интернет разведка\sample1.xlsx");
var excelPackage = new ExcelPackage();

SetNewExel(file, driver, linksToAssets, linksToAssetsExel);
excelPackage.SaveAs(file);
Clean();
Console.WriteLine("End");

#endregion

#region Methods defaults

void Initialize()
{
    try
    {
        Console.WriteLine("Initializing options");
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("log-level=3");
        // options.AddArgument("--ignore-certificate-errors-spki-list");
        // options.AddArgument("--ignore-certificate-errors");
        options.AddArguments("headless");

        Console.WriteLine("Initializing drivers");
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(300);
        driver.Manage().Window.Maximize();
        driver.Navigate().GoToUrl(Link());
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        driver?.Quit();
        Initialize();
        throw;
    }
}

void Clean()
{
    Console.WriteLine("Clean");
    driver.Close();
}

static void GetHrefAsset(IWebDriver driver, ref List<string> assets)
{
    Console.WriteLine("GetHrefAssets");
    var elements = driver.FindElements(By.ClassName("_3zZYp"));
    foreach (var element in elements)
    {
        var elem = element.FindElement(By.TagName("a")).GetAttribute("href");
        if (elem != null)
            assets.Add(elem);
        Console.WriteLine($"GetHrefAsset completed {elem} ");
    }
}

static (string, string, string, string) Scraping(IWebDriver driver)
{
    Console.WriteLine("Scraping");
    var elementAssetName = driver.FindElement(By.ClassName("cfm2v"));
    var elementAssetMoney = driver.FindElement(By.ClassName("_223RA"));
    var elementAssetSize = driver.FindElement(By.ClassName("product-size")).FindElement(By.ClassName("SoNzt"));
    var elementAssetDescription = driver.FindElement(By.ClassName("_1_3uP"));

    return (elementAssetName.Text, elementAssetMoney.Text, elementAssetSize.Text, elementAssetDescription.Text);
}

static void SetNewExel(FileInfo file, IWebDriver driver, List<string> linksToAssets, List<string> linksToAssetsExel)
{
    Console.WriteLine("SetNewExel");
    var excelPackage = new ExcelPackage(file);

    ExcelWorksheet worksheet;


    worksheet = excelPackage.Workbook.Worksheets.Add("DataBase Assets");

    worksheet = excelPackage.Workbook.Worksheets[0];

    worksheet.Cells["A1"].Value = "Url";
    worksheet.Cells["B1"].Value = "Name";
    worksheet.Cells["C1"].Value = "Money";
    worksheet.Cells["D1"].Value = "Size files";
    worksheet.Cells["E1"].Value = "Description asset";

    for (int i = 0; i < 100; i++)
    {
        Console.WriteLine($"SetNewExel Scraping {i}\t{linksToAssets[i]}");
        try
        {
            driver.Url = linksToAssets[i];
            var dataBase = Scraping(driver);

            worksheet.Cells[$"A{i + 2}"].Value = $"{driver.Url}";
            worksheet.Cells[$"B{i + 2}"].Value = $"{dataBase.Item1}";
            worksheet.Cells[$"C{i + 2}"].Value = $"{dataBase.Item2}";
            worksheet.Cells[$"D{i + 2}"].Value = $"{dataBase.Item3}";
            worksheet.Cells[$"E{i + 2}"].Value = $"{dataBase.Item4}";
        }
        catch (Exception e)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error {driver.Url}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            excelPackage.SaveAs(file);
        }
    }
}

static void SetSupplementExel(FileInfo file, IWebDriver driver, List<string> linksToAssets)
{
    var excelPackage = new ExcelPackage(file);
    var worksheet = excelPackage.Workbook.Worksheets[0];

    var linksToAssetsExel = new List<string>();

    int count = 0;
    for (;; count++)
    {
        if (worksheet.Cells[0, count].Value == null || (string)worksheet.Cells[0, count].Value == "")
            break;
        linksToAssetsExel.Add((string)worksheet.Cells[0, count].Value);
    }

    for (int i = 0; i < 2; i++)
    {
        try
        {
            if (linksToAssetsExel.Contains(linksToAssets[i]))
                continue;
            driver.Url = linksToAssets[i];
            var dataBase = Scraping(driver);

            worksheet.Cells[count, 0].Value = $"{driver.Url}";
            worksheet.Cells[count, 1].Value = $"{dataBase.Item1}";
            worksheet.Cells[count, 2].Value = $"{dataBase.Item2}";
            worksheet.Cells[count, 3].Value = $"{dataBase.Item3}";
            worksheet.Cells[count, 4].Value = $"{dataBase.Item4}";
            linksToAssetsExel.Add(driver.Url);
            count++;
        }
        catch (Exception e)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error {driver.Url}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(e);
            throw;
        }
    }
}

#endregion