using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class DetectCapability
{
    [JsonProperty("detected")]
    public bool Detected { get; set; }
}