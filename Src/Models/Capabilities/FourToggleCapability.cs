using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("toggle", Name = "4" )]
public class FourToggleCapability : ThreeToggleCapability
{
    [JsonProperty("4")]
    public ToggleState? Four { get;set; }
}