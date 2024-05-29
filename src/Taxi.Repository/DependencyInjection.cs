﻿using Microsoft.Extensions.DependencyInjection;
using Taxi.Core.Infrastructure;

namespace Taxi.Repository
{
    public class DependencyInjection
    {
        public static void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDriverRepository, DriverRepository>();
        }
    }
}
