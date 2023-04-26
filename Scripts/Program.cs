using ParsingMarketPlaces;
using ParsingMarketPlaces.Elements;

var browserWork = new BrowserWork();
browserWork.Driver!.Url = "https://assetstore.unity.com/3d";

Console.WriteLine(Search.ReferencesToAsset(browserWork.Driver));
Console.WriteLine(Search.ReferencesToAssets(browserWork.Driver)?.Length); //
Console.WriteLine(Search.CountResults(browserWork.Driver));

browserWork.Driver.Url =
    "https://assetstore.unity.com/packages/3d/environments/polygon-casino-low-poly-3d-art-by-synty-252731";

Console.WriteLine("1" + Packages.AssetName(browserWork.Driver));
Console.WriteLine("2" + Packages.AssetAuthor(browserWork.Driver));
Console.WriteLine("3" + Packages.AssetGrade(browserWork.Driver)); //
Console.WriteLine("4" + Packages.AssetFavorite(browserWork.Driver)); //
Console.WriteLine("5" + Packages.AssetPrice(browserWork.Driver));
Console.WriteLine("6" + Packages.AssetLicenseType(browserWork.Driver));
Console.WriteLine("7" + Packages.AssetFileSize(browserWork.Driver));
Console.WriteLine("8" + Packages.AssetLatestReleaseDate(browserWork.Driver));
Console.WriteLine("9" + Packages.AssetDescription(browserWork.Driver));