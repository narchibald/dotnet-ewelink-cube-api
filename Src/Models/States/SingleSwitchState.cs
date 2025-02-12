namespace EWeLink.Cube.Api.Models.States;

using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

public class SingleSwitchState : SwitchStateToggle
{
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
}