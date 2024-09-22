using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("toggle", Name = "3" )]
public class ThreeToggleCapability : TwoToggleCapability
{
    [JsonProperty("3")]
    public ToggleState? Three {get;set;}
}