namespace EWeLink.Cube.Api.Models.Capabilities;

using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

[Capability("motion")]
public class MotionCapability : Capability
{
    [JsonProperty("motionInterval")]
    [JsonConverter(typeof(IntToDecimalConverter), 2)]
    public decimal Interval { get; set; }
    
    [JsonProperty("motionSensitivity")]
    public MotionSensitivityState Sensitivity { get; set; }
}