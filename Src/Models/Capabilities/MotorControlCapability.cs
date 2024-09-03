using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class MotorControlCapability
{
    [JsonProperty("motorControl")]
    public MotorControlState Value { get; set; }
}