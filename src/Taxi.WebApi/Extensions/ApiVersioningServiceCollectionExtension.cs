using Asp.Versioning;

namespace Taxi.WebApi.Extensions;

/// <summary>
/// Contains extension methods to <see cref="IServiceCollection"/>
/// </summary>
public static class ApiVersioningServiceCollectionExtension
{
    public static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(option =>
            {
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = new ApiVersion(1, 0);
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}