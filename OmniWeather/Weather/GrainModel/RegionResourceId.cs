namespace OmniWeather.Weather.GrainModel;

[GenerateSerializer]
public sealed class RegionResourceId
{
    private static readonly string Delimiter = ";";

    public RegionResourceId(string name, string latitude, string longitude)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
    }

    public static RegionResourceId Parse(string region)
    {
        var parts = region.Split([Delimiter], StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3)
        {
            throw new ArgumentException($"Invalid region format: {region}. Expected format: 'name:latitude:longitude'.");
        }

        return new RegionResourceId(
             name: parts[0],
             latitude: parts[1],
             longitude: parts[2]
        );
    }
    
    public static implicit operator string(RegionResourceId resourceId) => resourceId.ToString();

    public override string ToString()
    {
        return $"{Name}{Delimiter}{Latitude}{Delimiter}{Longitude}";
    }

    [Id(0)]
    public string Name {get; set;}

    [Id(1)]
    public string Latitude {get;set;}

    [Id(2)]
    public string Longitude {get;set;}


}