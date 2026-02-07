namespace OmniWeather.Weather.ServiceModel;

public interface IForecastProvider
{
    Task<ForecastData> GetForecastAsync(double latitude, double longitude, int days);
}
