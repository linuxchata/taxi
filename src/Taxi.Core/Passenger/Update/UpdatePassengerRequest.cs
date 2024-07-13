using System.Diagnostics.CodeAnalysis;

namespace Taxi.Core.Passenger.Update
{
    public sealed class UpdatePassengerRequest
    {
        [NotNull]
        public string FirstName { get; set; } = null!;

        [NotNull]
        public string LastName { get; set; } = null!;

        [NotNull]
        public string Email { get; set; } = null!;

        [NotNull]
        public string PhoneNumber { get; set; } = null!;

        [NotNull]
        public string Country { get; set; } = null!;

        [NotNull]
        public string State { get; set; } = null!;

        [NotNull]
        public decimal Rating { get; set; }

        [NotNull]
        public bool IsActive { get; set; }
    }
}
