namespace ParsingMarketPlaces;

public class Asset
{
    public string Url = null;
    public string Name = null;
    public string? Author = null;
    public string? Price = null;
    public string? Rating = null;
    public string? NumberOfGrades = null;
    public string? Favorites = null;
    public string? LicenseAgreement = null;
    public string? LicenseType = null;
    public string? FileSize = null;
    public string? LatestVersion = null;
    public string? LatestReleaseDate = null;
    public string? OriginalUnityVersion = null;
    public string? Support = null;
    public List<string?> RelatedKeywords = new List<string?>();
    public string? Overview = null;
    public string? ReleasesOriginal = null;

    public Dictionary<string, string?> ReviewsStars = new()
    {
        { "5star", "" },
        { "4star", "" },
        { "3star", "" },
        { "2star", "" },
        { "1star", "" },
    };

    // public Image[] Image;

    public Asset()
    {
    }
}