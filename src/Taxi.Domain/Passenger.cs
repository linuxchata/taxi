using Newtonsoft.Json;

namespace Taxi.Domain
{
    public sealed class Passenger
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = null!;

        [JsonProperty(PropertyName = "pk")]
        public string Pk { get; set; } = null!;

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; } = null!;

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; } = null!;

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; } = null!;

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = null!;

        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }
    }
}
