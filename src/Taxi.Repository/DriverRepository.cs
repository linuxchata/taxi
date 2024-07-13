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

            try
            {
                var response = await container.ReadItemAsync<Driver>(id, new PartitionKey(id));

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

            await CreateTrigger(container);

            driver.Id = Guid.NewGuid().ToString().ToLower();
            driver.Pk = driver.Id;
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

            await CreateTrigger(container);

            driver.Id = id.ToString().ToLower();
            driver.Pk = driver.Id;
            driver.UpdatedDate = DateTime.UtcNow;

            try
            {
                var response = await container.ReplaceItemAsync<Driver>(
                    driver,
                    id.ToString(),
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

            var pk = id.ToString().ToLower();

            try
            {
                await container.DeleteItemAsync<Driver>(id.ToString(), new PartitionKey(pk));

                return true;
            }
            catch (CosmosException ex) when (IsNotFound(ex))
            {
                return false;
            }
        }

        private async Task CreateTrigger(Container container)
        {
            var triggerProperties = new TriggerProperties
            {
                Id = nameof(DriverPreTriggers.ValidateDriverPreTrigger),
                Body = DriverPreTriggers.ValidateDriverPreTrigger,
                TriggerOperation = TriggerOperation.All, // For Create & Update operation, All must be used
                TriggerType = TriggerType.Pre,
            };

            try
            {
                await container.Scripts.ReadTriggerAsync(nameof(DriverPreTriggers.ValidateDriverPreTrigger));
                await container.Scripts.ReplaceTriggerAsync(triggerProperties);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await container.Scripts.CreateTriggerAsync(triggerProperties);
                return;
            }
        }

        private bool IsNotFound(CosmosException ex)
        {
            return ex.StatusCode == System.Net.HttpStatusCode.NotFound;
        }
    }
}
