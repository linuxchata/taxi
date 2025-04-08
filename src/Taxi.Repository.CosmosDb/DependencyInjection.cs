using Microsoft.Extensions.DependencyInjection;
using Taxi.Core.Infrastructure;

namespace Taxi.Repository.CosmosDb;

public static class DependencyInjection
{
    public static void Register(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IDriverRepository, DriverRepository>();
        serviceCollection.AddTransient<IPassengerRepository, PassengerRepository>();
    }
}
