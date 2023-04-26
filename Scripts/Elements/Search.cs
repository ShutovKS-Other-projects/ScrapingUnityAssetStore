using OpenQA.Selenium;

namespace ParsingMarketPlaces.Elements;

public static class Search
{
    public static string? ReferencesToAsset(ISearchContext? driver)
    {
        if (Tools.TryElementSearch(driver, "_138_e", out var element))
        {
            if (Tools.TryHrefSearch(driver, element!, out var href))
            {
                return href;
            }
        }

        return null;
    }

    public static string[]? ReferencesToAssets(ISearchContext? driver)
    {
        if (Tools.TryElementsSearch(driver, "_138_e", out var elements))
        {
            var references = new List<string>();
            foreach (var element in elements!)
            {
                if (Tools.TryHrefSearch(driver, element, out var href))
                {
                    references.Add(href);
                }
            }

            return references.ToArray();
        }

        return null;
    }

    // public static string PagesCountMax(ISearchContext? driver) { }
    public static string? CountResults(ISearchContext? driver)
    {
        if (Tools.TryElementSearch(driver, "Hc-Sp", out var element))
        {
            var str = element!.Text;
            str = str.Remove(str.Length - 8, 8);
            str = str.Remove(0, 8);
            return str;
        }

        return null;
    }
}