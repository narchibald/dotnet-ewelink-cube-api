using EWeLink.Cube.Api.Models.Converters;
using EWeLink.Cube.Api.Models.States;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class TamperAlertCapability
{
    [JsonProperty("tamper")]
    public TamperState Value { get; set; }
}
