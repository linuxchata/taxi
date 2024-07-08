using System.Collections.Generic;
using Newtonsoft.Json;

namespace Taxi.Domain
{
    public sealed class Trip
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = null!;

        [JsonProperty(PropertyName = "pk")]
        public string Pk { get; set; } = null!;

        [JsonProperty(PropertyName = "driverId")]
        public string DriverId { get; set; } = null!;

        [JsonProperty(PropertyName = "driverName")]
        public string DriverName { get; set; } = null!;

        [JsonProperty(PropertyName = "vehicleId")]
        public string VehicleId { get; set; } = null!;

        [JsonProperty(PropertyName = "passengersIds")]
        public IEnumerable<string> PassengersIds { get; set; } = null!;

        [JsonProperty(PropertyName = "startDateTime")]
        public string StartDateTime { get; set; } = null!;

        [JsonProperty(PropertyName = "endDateTime")]
        public string EndDateTime { get; set; } = null!;

        [JsonProperty(PropertyName = "stops")]
        public GeoCoordinate[] Stops { get; set; } = null!;

        [JsonProperty(PropertyName = "distanceInKm")]
        public double DistanceInKm { get; set; }

        [JsonProperty(PropertyName = "rate")]
        public decimal Rate { get; set; }

        [JsonProperty(PropertyName = "totalCharge")]
        public decimal TotalCharge { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; } = null!;

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = nameof(Trip);

        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; } = 1;
    }
}
