using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("motor-reverse")]
public class MotorReverseCapability : Capability
{
    [JsonProperty("motorReverse")]
    public bool IsReversed { get; set; }
}