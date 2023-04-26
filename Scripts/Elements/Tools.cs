using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace ParsingMarketPlaces.Elements;

public static class Tools
{
    public static bool TryElementSearch(ISearchContext? driver, string searchClassName, out IWebElement? element)
    {
        var elements = driver?.FindElements(By.ClassName(searchClassName));
        if (elements != null && elements.Count != 0)
        {
            element = elements[0];
            return true;
        }

        element = null;
        return false;
    }

    public static bool TryElementsSearch(ISearchContext? driver, string searchClassName,
        out ReadOnlyCollection<IWebElement>? element)
    {
        var elements = driver?.FindElements(By.ClassName(searchClassName));
        if (elements != null && elements.Count != 0)
        {
            element = elements;
            return true;
        }

        element = null;
        return false;
    }

    public static bool TryHrefSearch(ISearchContext? driver,
        IWebElement element, out string href)
    {
        var elements = element.FindElements(By.TagName("a"));
        if (elements != null && elements.Count != 0)
        {
            href = elements[0].GetAttribute("href");
            if (href != null)
            {
                return true;
            }
        }

        href = "null";
        return false;
    }
}