using System.Collections.Generic;

namespace Taxi.Core.Passenger.GetAll
{
    public sealed class GetPassengersResponse
    {
        public IEnumerable<GetPassengerResponse> Items { get; set; } = null!;
    }
}
