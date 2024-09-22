using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("identify")]
public class IdentifyCapability : Capability
{
    [JsonProperty("identify")]
    public bool State { get; set; }
}