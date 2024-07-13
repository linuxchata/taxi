using Taxi.Core.Base;

namespace Taxi.Core.Passenger.Get
{
    public sealed class GetPassengerResponse : BaseResponse
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string State { get; set; } = null!;

        public decimal Rating { get; set; }

        public bool IsActive { get; set; }
    }
}
