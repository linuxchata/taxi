using GraphQL.Types;

namespace Taxi.WebApi.GraphQLQueries;

public sealed class DriverSchema : Schema
{
    public DriverSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<DriverGraphQLQuery>();
    }
}