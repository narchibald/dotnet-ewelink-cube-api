using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("toggle", Name = "1")]
public class OneToggleCapability : Capability
{
    [JsonProperty("1")]
    public ToggleState? One {get;set;}
}