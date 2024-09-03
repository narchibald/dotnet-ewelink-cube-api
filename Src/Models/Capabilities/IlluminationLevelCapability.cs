using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class IlluminationLevelCapability
{
    [JsonProperty("level")]
    public IlluminationLevel Level { get; set; }
}