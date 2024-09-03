using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class MotorReverseCapability
{
    [JsonProperty("motorReverse")]
    public bool IsReversed { get; set; }
}