using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Taxi.Core.Infrastructure;
using Taxi.Domain;

namespace Taxi.Repository
{
    public sealed class DriverRepository : IDriverRepository
    {
        private const string DatabaseId = "taxi";

        private const string Container = "people";

        private readonly IConfiguration _configuration;

        public DriverRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Driver>> GetAll()
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            var sql = "SELECT * FROM c";

            var iterator = container.GetItemQueryIterator<Driver>(sql);

            var documents = await iterator.ReadNextAsync();

            return documents.Resource;
        }

        public async Task<string> Create(Driver driver)
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            driver.Id = Guid.NewGuid();
            driver.Pk = GetPartitionKey(driver.State);

            var response = await container.CreateItemAsync<Driver>(driver);

            return response.Resource.Id.ToString();
        }

        public async Task Update(Guid id, string state, Driver driver)
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            driver.Id = id;
            driver.Pk = GetPartitionKey(driver.State);
            await container.ReplaceItemAsync<Driver>(driver, id.ToString());
        }

        public async Task Delete(Guid id, string state)
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            var pk = GetPartitionKey(state);
            await container.DeleteItemAsync<Driver>(id.ToString(), new PartitionKey(pk));
        }

        private string GetPartitionKey(string state)
        {
            return $"{nameof(Driver).ToLower()}_{state.ToLower()}";
        }
    }
}
