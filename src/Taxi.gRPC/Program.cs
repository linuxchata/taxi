using Microsoft.OpenApi.Models;
using Taxi.Core;
using Taxi.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Taxi gRPC", Version = "v1" });

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "Taxi.gRPC.xml");
    c.IncludeXmlComments(filePath);
    c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
});

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<DependencyInjection>());

Taxi.Repository.DependencyInjection.Register(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.

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
