using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class IdentifyCapability
{
    [JsonProperty("identify")]
    public bool State { get; set; }
}