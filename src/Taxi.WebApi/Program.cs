using System.Security.Authentication;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.FeatureManagement;
using Taxi.Repository;
using Taxi.WebApi.Extensions;
using Taxi.WebApi.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureHttpsDefaults(listenOptions =>
    {
        listenOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
    });
});

builder.Configuration.AddJsonFile("appsettings.local.json");

// Add services to the container
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddProblemDetails();
builder.Services.AddVersioning();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Taxi.Core.DependencyInjection>());
builder.Services.AddSwagger();

builder.Services.AddSecurity(builder.Configuration, builder.Environment);

builder.Services.AddHealthChecks();

builder.Services.AddFeatureManagement();

DependencyInjection.Register(builder.Services);

var app = builder.Build();

await Initialization.Init(app.Configuration);

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/healthcheck", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.Run();
