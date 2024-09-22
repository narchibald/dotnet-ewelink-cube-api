using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class ThreeStateToggle : SwitchStateToggle
{
    [JsonProperty("toggle")]
    public ThreeToggleCapability? Toggle { get; set; }
}