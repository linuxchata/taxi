using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace Taxi.Repository.CosmosDb;

public static class CosmosDbConnectionBuilder
{
    private static CosmosClient _client = null!;

    public static CosmosClient GetClient(IConfiguration configuration)
    {
        if (_client != null)
        {
            return _client;
        }

        var accountEndpoint = configuration["CosmosDb:AccountEndpoint"];
        var masterKey = configuration["CosmosDb:MasterKey"];

        _client = new CosmosClient(accountEndpoint, masterKey);

        return _client;
    }
}
