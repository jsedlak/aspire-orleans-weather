using OmniWeather.Weather.ViewModel;

namespace OmniWeather.Weather.ServiceModel;

public interface IGeocoder
{
    Task<GeocodeResult> GeocodeAsync(string location);
}
