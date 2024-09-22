using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("motor-control")]
public class MotorControlCapability : Capability 
{
    [JsonProperty("motorControl")]
    public MotorControlState Value { get; set; }
}