using System.Collections.Generic;
using Newtonsoft.Json;

namespace Taxi.Domain
{
    public sealed class Driver
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = null!;

        [JsonProperty(PropertyName = "pk")]
        public string Pk { get; set; } = null!;

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; } = null!;

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; } = null!;

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; } = null!;

        [JsonProperty(PropertyName = "phoneNumber")]
        public string PhoneNumber { get; set; } = null!;

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; } = null!;

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; } = null!;

        [JsonProperty(PropertyName = "rating")]
        public decimal Rating { get; set; }

        [JsonProperty(PropertyName = "vehicles")]
        public IEnumerable<DriverVehicle>? Vehicles { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = nameof(Driver);

        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; } = 1;
    }
}
