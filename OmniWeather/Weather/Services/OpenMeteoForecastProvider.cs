using System.Text.Json;
using System.Text.Json.Serialization;
using OmniWeather.Weather.ServiceModel;

namespace OmniWeather.Weather.Services;

public class OpenMeteoForecastProvider : IForecastProvider
{
    private static readonly string[] WeatherParameters = [

        "temperature_2m","weather_code","precipitation_probability","rain","snowfall"
    ];

    private static readonly Dictionary<int, string> WeatherCodeDescriptions = new()
    {
        { 0, "Clear sky" },
        { 1, "Mainly clear" },
        { 2, "Partly cloudy" },
        { 3, "Overcast" },
        { 45, "Fog" },
        { 48, "Depositing rime fog" },
        { 51, "Light drizzle" },
        { 53, "Moderate drizzle" },
        { 55, "Dense drizzle" },
        { 56, "Light freezing drizzle" },
        { 57, "Dense freezing drizzle" },
        { 61, "Slight rain" },
        { 63, "Moderate rain" },
        { 65, "Heavy rain" },
        { 66, "Light freezing rain" },
        { 67, "Heavy freezing rain" },
        { 71, "Slight snow fall" },
        { 73, "Moderate snow fall" },
        { 75, "Heavy snow fall" },
        { 77, "Snow grains" },
        { 80, "Slight rain showers" },
        { 81, "Moderate rain showers" },
        { 82, "Violent rain showers" },
        { 85, "Slight snow showers" },
        { 86, "Heavy snow showers" },
        { 95, "Thunderstorm: Slight or moderate" },
        { 96, "Thunderstorm with slight hail" },
        { 99, "Thunderstorm with heavy hail" }
    };

    private readonly HttpClient _httpClient;
    private readonly ILogger<IForecastProvider> _logger;

    private const string BaseUrl = "https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly={parameters}&timezone=auto&forecast_days={days}";

    public OpenMeteoForecastProvider(IHttpClientFactory httpClientFactory, ILogger<IForecastProvider> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
    }

    public Task<WeatherData[]> GetForecastAsync(double latitude, double longitude, int days)
    {
        var url = BaseUrl.Replace("{latitude}", latitude.ToString())
                         .Replace("{longitude}", longitude.ToString())
                         .Replace("{parameters}", string.Join(",", WeatherParameters))
                         .Replace("{days}", days.ToString());

        var response = _httpClient.GetAsync(url).Result;

        response.EnsureSuccessStatusCode();

        var content = response.Content.ReadAsStringAsync().Result;

        _logger.LogInformation(content);

        var result = JsonSerializer.Deserialize<OpenMeteoForecastResult>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        _logger.LogInformation(
            JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true })
        );

        var weatherData = new List<WeatherData>();

        for (var i = 0; i < result.Hourly.Time.Length; i++)
        {
            _logger.LogInformation($"Processing Index: {i}");
            _logger.LogInformation($"\tTime: {result.Hourly.Time[i]}");
            _logger.LogInformation($"\tTemp: {result.Hourly.Temperature2m[i]}");
            _logger.LogInformation($"\tWeatherCode: {result.Hourly.WeatherCode[i]}");
            _logger.LogInformation($"\tPrecipProb: {result.Hourly.PrecipitationProbability[i]}");
            _logger.LogInformation($"\tRain: {result.Hourly.Rain[i]}");
            _logger.LogInformation($"\tSnowfall: {result.Hourly.Snowfall[i]}");

            weatherData.Add(new WeatherData
            {
                Start = result.Hourly.Time[i],
                End = result.Hourly.Time[i].AddHours(1),
                Temperature = (float)result.Hourly.Temperature2m[i],
                Visibility = WeatherCodeDescriptions.ContainsKey(result.Hourly.WeatherCode[i]) ? WeatherCodeDescriptions[result.Hourly.WeatherCode[i]] : "Unknown",
                PrecipitationProbability = (float)result.Hourly.PrecipitationProbability[i],
                Rainfall = (float)result.Hourly.Rain[i],
                Snowfall = (float)result.Hourly.Snowfall[i]
            });
        }

        return Task.FromResult(weatherData.ToArray());
    }

    public class OpenMeteoForecastResult{
        [JsonPropertyName("hourly")] 
        public OpenMeteoHourlyData Hourly { get; set; }
    }

    public class OpenMeteoHourlyData
    {
        [JsonPropertyName("time")] 
        public DateTime[] Time { get; set; }

        [JsonPropertyName("temperature_2m")] 
        public double[] Temperature2m { get; set; }

        [JsonPropertyName("weather_code")] 
        public int[] WeatherCode { get; set; }

        [JsonPropertyName("precipitation_probability")] 
        public double[] PrecipitationProbability { get; set; }

        [JsonPropertyName("rain")] 
        public double[] Rain { get; set; }

        [JsonPropertyName("snowfall")] 
        public double[] Snowfall { get; set; }
    }
}