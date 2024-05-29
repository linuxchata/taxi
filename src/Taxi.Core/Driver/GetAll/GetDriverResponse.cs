using System;

namespace Taxi.Core.Driver.GetAll
{
    public sealed class GetDriverResponse
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string State { get; set; } = null!;
    }
}
