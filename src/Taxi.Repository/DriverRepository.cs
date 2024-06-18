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

        private const string Type = "Driver";

        private const int Version = 1;

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

            driver.Id = Guid.NewGuid().ToString().ToLower();
            driver.Pk = driver.Id;
            driver.Version = Version;
            driver.Type = Type;

            var response = await container.CreateItemAsync<Driver>(driver);

            return response.Resource.Id.ToString();
        }

        public async Task Update(string id, Driver driver)
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            driver.Id = id.ToString().ToLower();
            driver.Pk = id.ToString().ToLower();
            driver.Version = Version;
            driver.Type = Type;

            await container.ReplaceItemAsync<Driver>(driver, id.ToString());
        }

        public async Task Delete(string id)
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            var pk = id.ToString().ToLower();

            await container.DeleteItemAsync<Driver>(id.ToString(), new PartitionKey(pk));
        }
    }
}
