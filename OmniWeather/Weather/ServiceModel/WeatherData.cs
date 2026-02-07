namespace OmniWeather.Weather.ServiceModel;

public sealed class WeatherData
{
    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public float Temperature { get; set; }

    public string Visibility { get; set; } = string.Empty;

    public float PrecipitationProbability { get; set; }

    public float Rainfall { get; set; } 

    public float Snowfall { get; set; }
}