using System;
using Newtonsoft.Json;

namespace Taxi.Domain
{
    public sealed class Driver
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "pk")]
        public string Pk { get; set; } = null!;

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; } = null!;

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; } = null!;

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; } = null!;
    }
}
