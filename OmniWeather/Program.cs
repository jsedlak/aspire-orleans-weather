using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/forecast/{region}", async ([FromRoute]string region, [FromQuery]DateTimeOffset? date, [FromQuery]int? numberOfDays, [FromServices]IClusterClient client) =>
{
    var grain = client.GetGrain<OmniWeather.Weather.GrainModel.IRegionGrain>(region);

    return await grain.GetForecast(date, numberOfDays ?? 1);
})
.WithName("GetWeatherForecast");

app.Run();
