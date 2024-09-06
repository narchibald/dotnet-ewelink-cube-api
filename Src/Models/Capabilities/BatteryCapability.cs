using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class BatteryCapability
{
    [JsonProperty("battery")]
    [JsonConverter(typeof(IntToDecimalConverter), 0)]
    public decimal Value { get; set; }
}