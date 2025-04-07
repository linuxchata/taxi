using Newtonsoft.Json;

namespace Taxi.Domain;

public sealed class Vehicle
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = null!;

    [JsonProperty(PropertyName = "pk")]
    public string Pk { get; set; } = null!;

    [JsonProperty(PropertyName = "vehicleType")]
    public string VehicleType { get; set; } = null!;

    [JsonProperty(PropertyName = "model")]
    public string Model { get; set; } = null!;

    [JsonProperty(PropertyName = "registrationNumber")]
    public string RegistrationNumber { get; set; } = null!;

    [JsonProperty(PropertyName = "color")]
    public string Color { get; set; } = null!;

    [JsonProperty(PropertyName = "vehicleDrivers")]
    public IEnumerable<VehicleDriver>? VehicleDrivers { get; set; }

    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; } = nameof(Vehicle);

    [JsonProperty(PropertyName = "version")]
    public int Version { get; set; } = 1;
}
