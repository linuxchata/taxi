using Microsoft.OpenApi.Models;

namespace Taxi.gRPC.ServiceCollectionExtensions;

/// <summary>
/// Contains extension methods to <see cref="IServiceCollection"/>
/// </summary>
public static class SwaggerServiceCollectionExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddGrpcSwagger();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Taxi gRPC", Version = "v1" });

            var filePath = Path.Combine(System.AppContext.BaseDirectory, "Taxi.gRPC.xml");
            c.IncludeXmlComments(filePath);
            c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
        });

        return services;
    }
}
