using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class PressCapability
{
    [JsonProperty("press")]
    public PressState Value { get; set; }
}