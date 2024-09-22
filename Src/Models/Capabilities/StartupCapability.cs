using EWeLink.Cube.Api.Models.Converters;
using EWeLink.Cube.Api.Models.States;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("startup")]
public class StartupCapability : Capability
{
    [JsonProperty("startup")]
    public StartupState State { get; set; }
}