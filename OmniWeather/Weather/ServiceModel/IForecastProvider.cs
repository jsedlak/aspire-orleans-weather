namespace OmniWeather.Weather.ServiceModel;

public interface IForecastProvider
{
    Task<WeatherData[]> GetForecastAsync(double latitude, double longitude, int days);
}
