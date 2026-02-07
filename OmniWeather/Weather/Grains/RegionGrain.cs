using OmniWeather.Weather.GrainModel;
using OmniWeather.Weather.ServiceModel;
using OmniWeather.Weather.ViewModel;

namespace OmniWeather.Weather.Grains.RegionGrain;

public sealed class RegionGrain : Grain, IRegionGrain
{
    private readonly IForecastProvider _forecastProvider;

    public RegionGrain(IForecastProvider forecastProvider)
    {
        _forecastProvider = forecastProvider;
    }

    

    public async Task<Forecast[]> GetForecast(DateTimeOffset? date = null, int numberOfDays = 1)
    {
        var startDate = date ?? DateTimeOffset.UtcNow.Date;
        var numberOfDaysToGenerate = Math.Min(Math.Max(1, numberOfDays), 10);

        var forecast = await _forecastProvider.GetForecastAsync(
            double.Parse(ResourceId.Latitude), 
            double.Parse(ResourceId.Longitude), 
            numberOfDaysToGenerate
        );

        var days = forecast.GroupBy(m => m.Start.Date);

        var forecasts = new List<Forecast>();

        foreach(var day in days)
        {
            var parts = day.Select(d => new ForecastPart
            {
                Start = d.Start,
                End = d.End,
                IsMeasured = d.Start < DateTimeOffset.UtcNow,
                Visibility = d.Visibility,
                Temperature = (int)d.Temperature
            }).OrderBy(m => m.Start).ToArray();

            forecasts.Add(new Forecast
            {
                Region = ResourceId.ToString(),
                Date = day.Key,
                Parts = parts
            });
        }
        
        return forecasts.OrderBy(m => m.Date).ToArray();
        // var forecasts = new Forecast[numberOfDaysToGenerate];

        // var regionId = this.GetPrimaryKeyString();

        // for(var i = 0; i < numberOfDaysToGenerate; i++)
        // {
        //     var forecastDate = startDate.AddDays(i);
        //     forecasts[i] = new Forecast
        //     {
        //         Region = regionId,
        //         Date = forecastDate,
        //         Parts = Enumerable.Range(1, 24).Select(index => new ForecastPart
        //         {
        //             Start = forecastDate.AddHours(index - 1),
        //             End = forecastDate.AddHours(index),
        //             IsMeasured = forecastDate < DateTimeOffset.UtcNow.Date,
        //             Visibility = VisibilityOptions[Random.Shared.Next(VisibilityOptions.Length)],
        //             Temperature = Random.Shared.Next(-20, 55)
        //         }).ToArray()
        //     };
        // }
        
        // return Task.FromResult(forecasts);
    }

    private RegionResourceId? _resourceId;

    private RegionResourceId ResourceId
    {
        get
        {
            return _resourceId ??= RegionResourceId.Parse(this.GetPrimaryKeyString());
        }
    }
}