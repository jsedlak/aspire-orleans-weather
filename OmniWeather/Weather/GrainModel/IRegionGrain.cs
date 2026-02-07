using OmniWeather.Weather.ViewModel;

namespace OmniWeather.Weather.GrainModel;

public interface IRegionGrain : IGrainWithStringKey
{
    /// <summary>
    /// Gets the forecast for a given day and number of days that follow. Defaults to current day.
    /// </summary>
    /// <param name="date">The day being queried</param>
    /// <param name="numberOfDays">The number of days to look ahead from <see cref="date"/>. Minimum of 1.</param>
    Task<Forecast[]> GetForecast(DateTimeOffset? date = null, int numberOfDays = 1);
}