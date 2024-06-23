using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Taxi.Core.Infrastructure;
using Taxi.Domain;

namespace Taxi.Repository
{
    public sealed class PassengerRepository : IPassengerRepository
    {
        private const string DatabaseId = "taxi";

        private const string Container = "people";

        private const string Type = "Passenger";

        private const int Version = 1;

        private readonly IConfiguration _configuration;

        public PassengerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Passenger>> GetAll()
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            var sql = $"SELECT * FROM c WHERE c.type = '{Type}'";

            var iterator = container.GetItemQueryIterator<Passenger>(sql);

            var documents = await iterator.ReadNextAsync();

            return documents.Resource;
        }

        public async Task<Passenger> GetById(string id)
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            var passenger = await container.ReadItemAsync<Passenger>(id, new PartitionKey(id));

            return passenger.Resource;
        }

        public async Task<string> Create(Passenger passenger)
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            passenger.Id = Guid.NewGuid().ToString().ToLower();
            passenger.Pk = passenger.Id;
            passenger.Version = Version;
            passenger.Type = Type;

            var response = await container.CreateItemAsync<Passenger>(passenger);

            return response.Resource.Id.ToString();
        }

        public async Task Update(string id, Passenger passenger)
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            passenger.Id = id.ToString().ToLower();
            passenger.Pk = id.ToString().ToLower();
            passenger.Version = Version;
            passenger.Type = Type;

            await container.ReplaceItemAsync<Passenger>(passenger, id.ToString());
        }

        public async Task Delete(string id)
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            var pk = id.ToString().ToLower();

            await container.DeleteItemAsync<Passenger>(id.ToString(), new PartitionKey(pk));
        }
    }
}
