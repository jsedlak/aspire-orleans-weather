using OmniWeather.Weather.GrainModel;
using OmniWeather.Weather.ViewModel;

namespace OmniWeather.Weather.Grains.RegionGrain;

public sealed class RegionGrain : Grain, IRegionGrain
{
    private static readonly string[] VisibilityOptions = new[]
    {
        "Clear",
        "Partly Cloudy",
        "Cloudy",
        "Foggy",
        "Hazy",
        "Rainy",
        "Snowy"
    };

    public Task<Forecast[]> GetForecast(DateTimeOffset? date = null, int numberOfDays = 1)
    {
        var startDate = date ?? DateTimeOffset.UtcNow.Date;
        var numberOfDaysToGenerate = Math.Min(Math.Max(1, numberOfDays), 10);
        var forecasts = new Forecast[numberOfDaysToGenerate];

        var regionId = this.GetPrimaryKeyString();

        for(var i = 0; i < numberOfDaysToGenerate; i++)
        {
            var forecastDate = startDate.AddDays(i);
            forecasts[i] = new Forecast
            {
                Region = regionId,
                Date = forecastDate,
                Parts = Enumerable.Range(1, 24).Select(index => new ForecastPart
                {
                    Start = forecastDate.AddHours(index - 1),
                    End = forecastDate.AddHours(index),
                    IsMeasured = forecastDate < DateTimeOffset.UtcNow.Date,
                    Visibility = VisibilityOptions[Random.Shared.Next(VisibilityOptions.Length)],
                    Temperature = Random.Shared.Next(-20, 55)
                }).ToArray()
            };
        }
        
        return Task.FromResult(forecasts);
    }
}