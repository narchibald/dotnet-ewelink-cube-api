using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("color-rgb")]
public class ColorRgbCapability : Capability
{
    [JsonProperty("red")]
    [JsonConverter(typeof(IntToDecimalConverter), 0)]
    public decimal Red { get; set; }
    
    [JsonProperty("green")]
    [JsonConverter(typeof(IntToDecimalConverter), 0)]
    public decimal Green { get; set; }
    
    [JsonProperty("blue")]
    [JsonConverter(typeof(IntToDecimalConverter), 0)]
    public decimal Blue { get; set; }
}