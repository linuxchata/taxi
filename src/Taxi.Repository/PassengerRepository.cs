using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;
using Microsoft.Extensions.Configuration;
using Taxi.Core.Infrastructure;
using Taxi.Domain;
using Taxi.Repository.Triggers.Driver;

namespace Taxi.Repository
{
    public sealed class PassengerRepository : IPassengerRepository
    {
        private const string DatabaseId = "taxi";

        private const string Container = "people";

        private const string Type = "Passenger";

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

            await CreateTrigger(container);

            passenger.Id = Guid.NewGuid().ToString().ToLower();
            passenger.Pk = passenger.Id;

            var response = await container.CreateItemAsync<Passenger>(
                passenger,
                requestOptions: new ItemRequestOptions
                {
                    PreTriggers = new List<string>
                    {
                        nameof(PassengerPreTriggers.ValidatePassengerPreTrigger)
                    }
                });

            return response.Resource.Id.ToString();
        }

        public async Task Update(string id, Passenger passenger)
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            await CreateTrigger(container);

            passenger.Id = id.ToString().ToLower();
            passenger.Pk = id.ToString().ToLower();

            await container.ReplaceItemAsync<Passenger>(
                passenger,
                id.ToString(),
                requestOptions: new ItemRequestOptions
                {
                    PreTriggers = new List<string>
                    {
                        nameof(PassengerPreTriggers.ValidatePassengerPreTrigger)
                    }
                });
        }

        public async Task Delete(string id)
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            var pk = id.ToString().ToLower();

            await container.DeleteItemAsync<Passenger>(id.ToString(), new PartitionKey(pk));
        }

        private async Task CreateTrigger(Container container)
        {
            var triggerProperties = new TriggerProperties
            {
                Id = nameof(PassengerPreTriggers.ValidatePassengerPreTrigger),
                Body = PassengerPreTriggers.ValidatePassengerPreTrigger,
                TriggerOperation = TriggerOperation.All, // For Create & Update operation, All must be used
                TriggerType = TriggerType.Pre,
            };

            try
            {
                await container.Scripts.ReadTriggerAsync(nameof(PassengerPreTriggers.ValidatePassengerPreTrigger));
                await container.Scripts.ReplaceTriggerAsync(triggerProperties);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await container.Scripts.CreateTriggerAsync(triggerProperties);
                return;
            }
        }
    }
}
