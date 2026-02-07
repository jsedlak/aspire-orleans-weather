namespace OmniWeather.Weather.ServiceModel;

public sealed class ForecastData
{
    public WeatherData[] WeatherDataPoints { get; set; } = Array.Empty<WeatherData>();
}
