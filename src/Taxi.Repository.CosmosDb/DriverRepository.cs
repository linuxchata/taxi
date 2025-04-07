using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Taxi.Core.Infrastructure;
using Taxi.Domain;
using Taxi.Repository.CosmosDb.Triggers.Driver;

namespace Taxi.Repository.CosmosDb;

public sealed class DriverRepository : IDriverRepository
{
    private const string DatabaseId = "taxi";

    private const string Container = "people";

    private const string Type = "Driver";

    private readonly IConfiguration _configuration;

    public DriverRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<Driver>> GetAll()
    {
        var client = CosmosDbConnectionBuilder.GetClient(_configuration);
        var container = client.GetContainer(DatabaseId, Container);

        var query = new QueryDefinition("SELECT * FROM c WHERE c.type = @type")
            .WithParameter("@type", Type);

        using var iterator = container.GetItemQueryIterator<Driver>(query);

        var documents = await iterator.ReadNextAsync();

        return documents.Resource;
    }

    public async Task<Driver> GetById(string id)
    {
        var client = CosmosDbConnectionBuilder.GetClient(_configuration);
        var container = client.GetContainer(DatabaseId, Container);

        var pk = new PartitionKey(BuildPartitionKey(id));

        try
        {
            var response = await container.ReadItemAsync<Driver>(id, pk);

            return response.Resource;
        }
        catch (CosmosException ex) when (IsNotFound(ex))
        {
            return null!;
        }
    }

    public async Task<string> Create(Driver driver)
    {
        var client = CosmosDbConnectionBuilder.GetClient(_configuration);
        var container = client.GetContainer(DatabaseId, Container);

        driver.Id = Guid.NewGuid().ToString().ToLower();
        driver.Pk = BuildPartitionKey(driver.Id);
        driver.CreatedDate = DateTime.UtcNow;
        driver.UpdatedDate = driver.CreatedDate;

        var response = await container.CreateItemAsync(
            driver,
            requestOptions: new ItemRequestOptions
            {
                PreTriggers = new List<string>
                {
                    nameof(DriverPreTriggers.ValidateDriverPreTrigger)
                }
            });

        return response.Resource.Id.ToString();
    }

    public async Task<Driver> Update(string id, Driver driver)
    {
        var client = CosmosDbConnectionBuilder.GetClient(_configuration);
        var container = client.GetContainer(DatabaseId, Container);

        driver.Id = id.ToLower();
        driver.Pk = BuildPartitionKey(driver.Id);
        driver.UpdatedDate = DateTime.UtcNow;

        try
        {
            var response = await container.ReplaceItemAsync(
                driver,
                id,
                requestOptions: new ItemRequestOptions
                {
                    PreTriggers = new List<string>
                    {
                        nameof(DriverPreTriggers.ValidateDriverPreTrigger)
                    }
                });

            return response.Resource;
        }
        catch (CosmosException ex) when (IsNotFound(ex))
        {
            return null!;
        }
    }

    public async Task<bool> Delete(string id)
    {
        var client = CosmosDbConnectionBuilder.GetClient(_configuration);
        var container = client.GetContainer(DatabaseId, Container);

        var pk = new PartitionKey(BuildPartitionKey(id));

        try
        {
            await container.DeleteItemAsync<Driver>(id, pk);

            return true;
        }
        catch (CosmosException ex) when (IsNotFound(ex))
        {
            return false;
        }
    }

    private static string BuildPartitionKey(string id)
    {
        return $"{Type}:{id.ToLower()}";
    }

    private static bool IsNotFound(CosmosException ex)
    {
        return ex.StatusCode == System.Net.HttpStatusCode.NotFound;
    }
}
