using Taxi.Core;
using Taxi.gRPC.ServiceCollectionExtensions;
using Taxi.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.local.json", optional: true);

// Add services to the container
builder.Services.AddGrpc();
builder.Services.AddSwagger();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<DependencyInjection>());

Taxi.Repository.DependencyInjection.Register(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taxi gRPC v1");
    });
}

app.MapGrpcService<DriverService>();
app.MapGrpcService<PassengerService>();
app.MapGet("/", () => "gRPC endpoints are up and running");

app.Run();
