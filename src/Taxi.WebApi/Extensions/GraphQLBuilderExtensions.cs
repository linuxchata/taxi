using Taxi.WebApi.GraphQLQueries;

namespace Taxi.WebApi.Extensions
{
    public static class GraphQLBuilderExtensions
    {
        public static IApplicationBuilder UseGraphQL(this IApplicationBuilder app)
        {
            app.UseGraphQL<DriverSchema>("/graphql/drivers");
            app.UseGraphQL<PassengerSchema>("/graphql/passengers");
            app.UseGraphQLPlayground(
                "/drivers",
                new GraphQL.Server.Ui.Playground.PlaygroundOptions
                {
                    GraphQLEndPoint = "../graphql",
                    SubscriptionsEndPoint = "../graphql",
                });
            app.UseGraphQLPlayground(
                "/passengers",
                new GraphQL.Server.Ui.Playground.PlaygroundOptions
                {
                    GraphQLEndPoint = "../graphql",
                    SubscriptionsEndPoint = "../graphql",
                });

            return app;
        }
    }
}
