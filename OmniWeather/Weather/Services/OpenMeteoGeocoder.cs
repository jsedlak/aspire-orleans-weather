using System.Text.Json;
using Newtonsoft.Json;
using OmniWeather.Weather.ServiceModel;

namespace OmniWeather.Weather.Services;

public class OpenMeteoGeocoder : IGeocoder
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IGeocoder> _logger;

    private const string BaseUrl = "https://geocoding-api.open-meteo.com/v1/search?name={location}&count=1";

    public OpenMeteoGeocoder(IHttpClientFactory httpClientFactory, ILogger<IGeocoder> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
    }

    public Task<GeocodeResult> GeocodeAsync(string location)
    {
        var url = BaseUrl.Replace("{location}", Uri.EscapeDataString(location));

        var response = _httpClient.GetAsync(url).Result;
        
        response.EnsureSuccessStatusCode();

        var content = response.Content.ReadAsStringAsync().Result;
        var result = JsonConvert.DeserializeObject<OpenMeteoGeocodeResult>(content);

        if (result.Results.Count() == 0) 
        {
            throw new Exception($"Location '{location}' not found.");
        }

        var firstResult = result.Results[0];
        return Task.FromResult(new GeocodeResult(firstResult.Name, firstResult.Latitude, firstResult.Longitude));
    }

    public record struct OpenMeteoGeocodeResult(
        [JsonProperty("results")]OpenMeteoGeocodeEntry[] Results
    );

    public record struct OpenMeteoGeocodeEntry(
        [JsonProperty("name")]string Name, 
        [JsonProperty("latitude")]double Latitude, 
        [JsonProperty("longitude")]double Longitude, 
        [JsonProperty("timezone")]string Timezone, 
        [JsonProperty("country_code")] string CountryCode
    );

}
