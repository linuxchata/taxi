using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Taxi.Core.Infrastructure;
using Taxi.Domain;
using Taxi.Repository.CosmosDb.Triggers.Passenger;

namespace Taxi.Repository.CosmosDb;
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
            var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            var query = new QueryDefinition("SELECT * FROM c WHERE c.type = @type")
                .WithParameter("@type", Type);

            var iterator = container.GetItemQueryIterator<Passenger>(query);

            var documents = await iterator.ReadNextAsync();

            return documents.Resource;
        }

        public async Task<Passenger> GetById(string id)
        {
            var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            var pk = new PartitionKey(BuildPartitionKey(id));

            try
            {
                var response = await container.ReadItemAsync<Passenger>(id, pk);

                return response.Resource;
            }
            catch (CosmosException ex) when (IsNotFound(ex))
            {
                return null!;
            }
        }

        public async Task<string> Create(Passenger passenger)
        {
            var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            passenger.Id = Guid.NewGuid().ToString().ToLower();
            passenger.Pk = BuildPartitionKey(passenger.Id);
            passenger.CreatedDate = DateTime.UtcNow;
            passenger.UpdatedDate = passenger.CreatedDate;

            var response = await container.CreateItemAsync(
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

        public async Task<Passenger> Update(string id, Passenger passenger)
        {
            var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            passenger.Id = id.ToLower();
            passenger.Pk = BuildPartitionKey(passenger.Id);
            passenger.UpdatedDate = DateTime.UtcNow;

            try
            {
                var response = await container.ReplaceItemAsync(
                 passenger,
                 id,
                 requestOptions: new ItemRequestOptions
                 {
                     PreTriggers = new List<string>
                     {
                        nameof(PassengerPreTriggers.ValidatePassengerPreTrigger)
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
                await container.DeleteItemAsync<Passenger>(id, pk);

                return true;
            }
            catch (CosmosException ex) when (IsNotFound(ex))
            {
                return false;
            }
        }

        private string BuildPartitionKey(string id)
        {
            return $"{Type}:{id.ToLower()}";
        }

        private bool IsNotFound(CosmosException ex)
        {
            return ex.StatusCode == System.Net.HttpStatusCode.NotFound;
        }
    }
}
