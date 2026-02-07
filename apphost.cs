#:package Aspire.Hosting.JavaScript@13.1.0
#:package Aspire.Hosting.Orleans@13.1.0
#:sdk Aspire.AppHost.Sdk@13.1.0
#:project OmniWeather/OmniWeather.csproj

var builder = DistributedApplication.CreateBuilder(args);

var orleans = builder.AddOrleans("orleans")
    .WithMemoryGrainStorage("Default")
    .WithDevelopmentClustering();

var api = builder.AddProject<Projects.OmniWeather>("api")
    .WithReference(orleans);

builder.AddJavaScriptApp("omniapp", "./omni-app")
    .WithReference(api, "API")
    .WithHttpEndpoint(env: "PORT");
    

builder.Build().Run();
