using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Taxi.Domain;

namespace Taxi.Core.Driver.Update
{
    public sealed class UpdateDriverRequest
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

        [NotNull]
        public bool IsApproved { get; set; }

        [NotNull]
        public IEnumerable<DriverVehicle>? Vehicles { get; set; }
    }
}
