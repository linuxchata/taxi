using Newtonsoft.Json;

namespace Taxi.Domain
{
    public sealed class DriverVehicle
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = null!;

        [JsonProperty(PropertyName = "vehicleType")]
        public string VehicleType { get; set; } = null!;

        [JsonProperty(PropertyName = "model")]
        public string Model { get; set; } = null!;

        [JsonProperty(PropertyName = "registrationNumber")]
        public string RegistrationNumber { get; set; } = null!;

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; } = null!;
    }
}
