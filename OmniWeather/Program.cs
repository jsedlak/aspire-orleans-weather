using Microsoft.AspNetCore.Mvc;
using OmniWeather.Weather.GrainModel;
using OmniWeather.Weather.ServiceModel;
using OmniWeather.Weather.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddScoped<IGeocoder, OpenMeteoGeocoder>();
builder.Services.AddScoped<IForecastProvider, OpenMeteoForecastProvider>();
builder.UseOrleans();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/forecast/{region}", async (
    [FromRoute]string region, 
    [FromQuery]DateTimeOffset? date, 
    [FromQuery]int? numberOfDays, 
    [FromServices]IClusterClient client,
    [FromServices]IGeocoder geocoder) =>
{
    var geocodeResult = await geocoder.GeocodeAsync(region);
    var resourceId = new RegionResourceId(geocodeResult.Name, geocodeResult.Latitude.ToString(), geocodeResult.Longitude.ToString());

    var grain = client.GetGrain<IRegionGrain>(resourceId);

    return await grain.GetForecast(date, numberOfDays ?? 1);
})
.WithName("GetWeatherForecast");

app.Run();
