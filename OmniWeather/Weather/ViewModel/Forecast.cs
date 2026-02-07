namespace OmniWeather.Weather.ViewModel;

[GenerateSerializer]
public sealed class Forecast
{
    [Id(0)]
    public required string Region { get; init; }

    [Id(1)]
    public DateTimeOffset Date {get;set;} = DateTimeOffset.UtcNow.Date;

    [Id(2)]
    public ForecastPart[] Parts {get;set; } = Array.Empty<ForecastPart>();
}
