using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("detect")]
public class DetectCapability : Capability
{
    [JsonProperty("detected")]
    public bool Detected { get; set; }
}