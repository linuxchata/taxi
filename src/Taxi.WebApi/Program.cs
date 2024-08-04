using System.Net;
using System.Reflection;
using System.Security.Authentication;
using Asp.Versioning;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Taxi.Repository;
using Taxi.WebApi.Authentication;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureHttpsDefaults(listenOptions =>
    {
        listenOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
    });
});

// Add services to the container
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new ApiVersion(1, 0);
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Taxi.Core.DependencyInjection>());
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    c.AddSecurityDefinition(SwaggerSecurity.ApiKeyHeader, new OpenApiSecurityScheme()
    {
        Name = Header.ApiKey,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = $"Authorization by {Header.ApiKey} request's header",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                Id = SwaggerSecurity.ApiKeyHeader,
                Type = ReferenceType.SecurityScheme,
                },
            },
            new string[] {}
        }
    });
});

var apiKey = builder.Configuration.GetValue<string>("ApiKey")!.ToSecureString();
builder.Services
    .AddAuthentication(Scheme.ApiKey)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
        Scheme.ApiKey,
        x => x.ApiKey = new NetworkCredential(string.Empty, apiKey).SecurePassword);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policy.ApiKey, policy =>
    {
        policy.AddAuthenticationSchemes(Scheme.ApiKey);
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddHealthChecks();

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
