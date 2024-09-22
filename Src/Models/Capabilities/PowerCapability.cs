using EWeLink.Cube.Api.Models.Converters;
using EWeLink.Cube.Api.Models.States;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("power")]
public class PowerCapability : Capability
{
    [JsonProperty("powerState")]
    public SwitchState State { get; set; }
}