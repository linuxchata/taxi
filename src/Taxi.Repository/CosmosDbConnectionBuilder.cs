using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace Taxi.Repository
{
    public static class CosmosDbConnectionBuilder
    {
        public static CosmosClient GetClient(IConfiguration configuration)
        {
            var accountEndpoint = configuration["CosmosDb:AccountEndpoint"];
            var masterKey = configuration["CosmosDb:MasterKey"];

            return new CosmosClient(accountEndpoint, masterKey);
        }
    }
}
