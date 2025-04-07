using Azure.Identity;
using Taxi.Core;
using Taxi.gRPC.ServiceCollectionExtensions;
using Taxi.gRPC.Services;
using Taxi.Repository.CosmosDb;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddGrpc();
builder.Services.AddSwagger();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<DependencyInjection>());

Taxi.Repository.CosmosDb.DependencyInjection.Register(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taxi gRPC v1");
});

app.MapGrpcService<DriverService>();
app.MapGrpcService<PassengerService>();
app.MapGet("/", () => "gRPC endpoints are up and running");

app.Run();
