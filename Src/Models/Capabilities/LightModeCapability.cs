namespace EWeLink.Cube.Api.Models.Capabilities;

using Newtonsoft.Json;

[Capability("mode")]
public class LightModeCapability : Capability
{
    [JsonProperty("lightMode")]
    public LightMode LightMode { get; set; }
}