using System.Reflection;
using Microsoft.OpenApi.Models;
using Taxi.WebApi.Authentication;

namespace Taxi.WebApi.ServiceCollectionExtensions;

/// <summary>
/// Contains extension methods to <see cref="IServiceCollection"/>
/// </summary>
public static class SwaggerServiceCollectionExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
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

        return services;
    }
}
