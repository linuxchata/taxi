using System;

namespace Taxi.Core.Passenger.GetAll
{
    public sealed class GetPassengerResponse
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string State { get; set; } = null!;
    }
}
