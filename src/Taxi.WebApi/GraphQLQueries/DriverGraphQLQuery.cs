using GraphQL;
using GraphQL.Types;
using Taxi.Core.Infrastructure;
using Taxi.Domain;
using Taxi.WebApi.Authentication;

namespace Taxi.WebApi.GraphQLQueries;

public sealed class DriverGraphQLQuery : ObjectGraphType
{
    public DriverGraphQLQuery(IDriverRepository driverRepository)
    {
        this.AuthorizeWithPolicy(Policy.ApiKey);

        Field<ListGraphType<DriverType>>("drivers")
            .ResolveAsync(async context => await driverRepository.GetAll());

        Field<DriverType>("driver")
            .Argument<NonNullGraphType<StringGraphType>>("id", "Identifier of the driver")
            .ResolveAsync(async context => await driverRepository.GetById(context.GetArgument<string>("id")));
    }
}