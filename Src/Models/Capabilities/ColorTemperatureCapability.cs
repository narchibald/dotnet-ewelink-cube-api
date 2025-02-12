using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("color-temperature")]
public class ColorTemperatureCapability : Capability
{
    [JsonProperty("colorTemperature")]
    [JsonConverter(typeof(IntToDecimalConverter), 0)]
    public decimal Value { get; set; }
}