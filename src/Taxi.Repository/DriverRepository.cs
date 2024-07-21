using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Taxi.Core.Infrastructure;
using Taxi.Domain;
using Taxi.Repository.Triggers.Driver;

namespace Taxi.Repository
{
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
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            var sql = $"SELECT * FROM c WHERE c.type = '{Type}'";

            var iterator = container.GetItemQueryIterator<Driver>(sql);

            var documents = await iterator.ReadNextAsync();

            return documents.Resource;
        }

        public async Task<Driver> GetById(string id)
        {
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
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
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            driver.Id = Guid.NewGuid().ToString().ToLower();
            driver.Pk = BuildPartitionKey(driver.Id);
            driver.CreatedDate = DateTime.UtcNow;
            driver.UpdatedDate = driver.CreatedDate;

            var response = await container.CreateItemAsync<Driver>(
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
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
            var container = client.GetContainer(DatabaseId, Container);

            driver.Id = id.ToLower();
            driver.Pk = BuildPartitionKey(driver.Id);
            driver.UpdatedDate = DateTime.UtcNow;

            try
            {
                var response = await container.ReplaceItemAsync<Driver>(
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
            using var client = CosmosDbConnectionBuilder.GetClient(_configuration);
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
