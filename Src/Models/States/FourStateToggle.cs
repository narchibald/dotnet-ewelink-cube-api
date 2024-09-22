using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class FourStateToggle : SwitchStateToggle
{
    [JsonProperty("toggle")]
    public FourToggleCapability? Toggle { get; set; }
}