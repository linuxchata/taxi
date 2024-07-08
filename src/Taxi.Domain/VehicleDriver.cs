using Newtonsoft.Json;

namespace Taxi.Domain
{
    public sealed class VehicleDriver
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = null!;

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; } = null!;

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; } = null!;
    }
}
