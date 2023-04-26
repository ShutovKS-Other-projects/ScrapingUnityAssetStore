namespace ParsingMarketPlaces;

public class AssetStoreSearchSetting
{
    private readonly string _searchForAssets;
    private readonly int _priceMinimum;
    private readonly int _priceMaximum;
    private readonly bool _isFreePrice;
    private readonly UnityVersion _unityVersion;
    private readonly int _ratings;
}

public enum UnityVersion
{
    All,
    Unity2022,
    Unity2021,
    Unity2020,
    Unity2019,
    Unity2018,
    Unity2017,
    Unity5
}