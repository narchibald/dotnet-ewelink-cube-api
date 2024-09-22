using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("press")]
public class PressCapability : Capability
{
    [JsonProperty("press")]
    public PressState Value { get; set; }
}