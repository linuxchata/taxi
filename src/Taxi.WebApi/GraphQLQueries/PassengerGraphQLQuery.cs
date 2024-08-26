using GraphQL;
using GraphQL.Types;
using Taxi.Core.Infrastructure;
using Taxi.Domain;
using Taxi.WebApi.Authentication;

namespace Taxi.WebApi.GraphQLQueries;

public sealed class PassengerGraphQLQuery : ObjectGraphType
{
    public PassengerGraphQLQuery(IPassengerRepository passengerRepository)
    {
        this.AuthorizeWithPolicy(Policy.ApiKey);

        Field<ListGraphType<PassengerType>>("passengers")
            .ResolveAsync(async context => await passengerRepository.GetAll());

        Field<PassengerType>("passenger")
            .Argument<NonNullGraphType<StringGraphType>>("id", "Identifier of the passenger")
            .ResolveAsync(async context => await passengerRepository.GetById(context.GetArgument<string>("id")));
    }
}