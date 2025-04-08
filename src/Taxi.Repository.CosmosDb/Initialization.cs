using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;
using Microsoft.Extensions.Configuration;
using Taxi.Repository.CosmosDb.Triggers.Driver;
using Taxi.Repository.CosmosDb.Triggers.Passenger;

namespace Taxi.Repository.CosmosDb;

public static class Initialization
{
    private const string DatabaseId = "taxi";

    private const string Container = "people";

    public static async Task Init(IConfiguration configuration)
    {
        var client = CosmosDbConnectionBuilder.GetClient(configuration);
        var container = client.GetContainer(DatabaseId, Container);

        await CreatePreTrigger(
            container,
            nameof(DriverPreTriggers.ValidateDriverPreTrigger),
            DriverPreTriggers.ValidateDriverPreTrigger);

        await CreatePreTrigger(
            container,
            nameof(PassengerPreTriggers.ValidatePassengerPreTrigger),
            PassengerPreTriggers.ValidatePassengerPreTrigger);
    }

    private static async Task CreatePreTrigger(Container container, string triggerId, string trigger)
    {
        var triggerProperties = new TriggerProperties
        {
            Id = triggerId,
            Body = trigger,
            TriggerOperation = TriggerOperation.All, // For Create & Update operation, All must be used
            TriggerType = TriggerType.Pre,
        };

        try
        {
            await container.Scripts.ReadTriggerAsync(triggerId);
            await container.Scripts.ReplaceTriggerAsync(triggerProperties);
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await container.Scripts.CreateTriggerAsync(triggerProperties);
        }
    }
}
