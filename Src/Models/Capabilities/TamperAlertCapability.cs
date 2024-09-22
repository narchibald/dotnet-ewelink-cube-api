using EWeLink.Cube.Api.Models.Converters;
using EWeLink.Cube.Api.Models.States;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("tamper-alert")]
public class TamperAlertCapability : Capability
{
    [JsonProperty("tamper")]
    public TamperState Value { get; set; }
}
