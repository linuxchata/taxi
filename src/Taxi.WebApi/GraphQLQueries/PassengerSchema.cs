using GraphQL.Types;

namespace Taxi.WebApi.GraphQLQueries;

public sealed class PassengerSchema : Schema
{
    public PassengerSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<PassengerGraphQLQuery>();
    }
}
