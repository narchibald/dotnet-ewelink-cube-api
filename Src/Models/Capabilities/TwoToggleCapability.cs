using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("toggle", Name = "2" )]
public class TwoToggleCapability : OneToggleCapability
{
    [JsonProperty("2")]
    public ToggleState? Two {get;set;}
}