using OpenQA.Selenium;

namespace ParsingMarketPlaces.Elements;

public static class Packages
{
    public static string? AssetName(ISearchContext? driver)
    {
        if (Tools.TryElementSearch(driver, "cfm2v", out var element))
        {
            return element!.Text;
        }

        return null;
    }

    public static string? AssetAuthor(ISearchContext? driver)
    {
        if (Tools.TryElementSearch(driver, "U9Sw1", out var element))
        {
            return element!.Text;
        }

        return null;
    }

    public static string? AssetGrade(ISearchContext? driver)
    {
        if (Tools.TryElementSearch(driver, "NoXio", out var element))
        {
            return element!.Text;
        }

        return null;
    }

    public static string? AssetFavorite(ISearchContext? driver) ////////////////
    {
        if (Tools.TryElementSearch(driver, "ifont-favorite", out var element))
        {
            return element!.Text;
        }

        return null;
    }

    public static string? AssetPrice(ISearchContext? driver)
    {
        if (Tools.TryElementSearch(driver, "mErEH", out var element))
        {
            return element!.Text;
        }

        return null;
    }

    public static string? AssetLicenseType(ISearchContext? driver)
    {
        if (Tools.TryElementSearch(driver, "product-license", out var element))
        {
            if (Tools.TryElementSearch(driver, "SoNzt", out element))
            {
                return element!.Text;
            }
        }

        return null;
    }

    public static string? AssetFileSize(ISearchContext? driver)
    {
        if (Tools.TryElementSearch(driver, "product-size", out var element))
        {
            if (Tools.TryElementSearch(driver, "SoNzt", out element))
            {
                return element!.Text;
            }
        }

        return null;
    }

    public static string? AssetLatestReleaseDate(ISearchContext? driver)
    {
        if (Tools.TryElementSearch(driver, "product-date", out var element))
        {
            if (Tools.TryElementSearch(driver, "SoNzt", out element))
            {
                return element!.Text;
            }
        }

        return null;
    }

    public static string? AssetDescription(ISearchContext? driver)
    {
        if (Tools.TryElementSearch(driver, "_1_3uP", out var element))
        {
            return element!.Text;
        }

        return null;
    }
}