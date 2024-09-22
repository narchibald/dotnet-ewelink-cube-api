using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("illumination-level")]
public class IlluminationLevelCapability : Capability
{
    [JsonProperty("level")]
    public IlluminationLevel Level { get; set; }
}