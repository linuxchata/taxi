using GraphQL;
using GraphQL.Server.Transports.AspNetCore;
using Taxi.Domain;
using Taxi.WebApi.GraphQLQueries;

namespace Taxi.WebApi.Extensions;

public static class GraphQLServiceCollectionExtension
{
    public static IServiceCollection AddGraphQL(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddSingleton<DriverType>();
        services.AddSingleton<PassengerType>();
        services.AddSingleton<DriverGraphQLQuery>();
        services.AddSingleton<PassengerGraphQLQuery>();

        services
            .AddGraphQL(b => b
                .AddSchema<DriverSchema>()
                .AddSchema<PassengerSchema>()
                .AddSystemTextJson()
                .AddValidationRule<AuthorizationValidationRule>());

        return services;
    }
}
