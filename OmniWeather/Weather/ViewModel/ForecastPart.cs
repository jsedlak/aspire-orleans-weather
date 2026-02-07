namespace OmniWeather.Weather.ViewModel;

[GenerateSerializer]
public sealed class ForecastPart
{
    [Id(0)]
    public DateTimeOffset Start {get;set;}

    [Id(1)]
    public DateTimeOffset End {get;set;}

    [Id(2)]
    public bool IsMeasured {get;set;}

    /// <summary>
    /// Gets or Sets the temperature in celsius
    /// </summary>
    [Id(3)]
    public float Temperature{get;set;}

    /// <summary>
    /// Gets or Sets the visibility (cloudy, partly cloudy, etc)
    /// </summary>
    [Id(4)]
    public string Visibility {get;set;} = string.Empty;
}