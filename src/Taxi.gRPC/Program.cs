using Taxi.Core;
using Taxi.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<DependencyInjection>());

Taxi.Repository.DependencyInjection.Register(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DriverService>();
app.MapGrpcService<PassengerService>();
app.MapGet("/", () => "gRPC endpoints are up and running");

app.Run();
