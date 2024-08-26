using System.Security.Authentication;
using Azure.Identity;
using GraphQL;
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

builder.Configuration.AddJsonFile("appsettings.local.json", optional: true);

if (builder.Environment.IsProduction())
{
    var keyVaultName = builder.Configuration["KeyVaultName"];
    builder.Configuration
        .AddAzureKeyVault(
            new Uri($"https://{keyVaultName}.vault.azure.net/"),
            new DefaultAzureCredential());
}

// Add services to the container
builder.Services.AddHttpContextAccessor(); // For GraphQL authorization
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddProblemDetails();
builder.Services.AddVersioning();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Taxi.Core.DependencyInjection>());

builder.Services.AddGraphQL();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseGraphQL();

app.MapHealthChecks("/healthcheck", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
    }
});

app.MapGet("/", () => "REST API endpoints are up and running");

app.Run();
