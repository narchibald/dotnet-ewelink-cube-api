using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class TwoStateToggle : SwitchStateToggle
{
    [JsonProperty("toggle")]
    public TwoToggleCapability? Toggle { get; set; }
}