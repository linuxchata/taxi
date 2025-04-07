using Newtonsoft.Json;

namespace Taxi.Domain;

public sealed class GeoCoordinate
{
    [JsonProperty(PropertyName = "latitude")]
    public double Latitude { get; set; }

    [JsonProperty(PropertyName = "longitude")]
    public double Longitude { get; set; }
}
