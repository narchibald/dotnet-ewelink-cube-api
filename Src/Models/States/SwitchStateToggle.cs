using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public abstract class SwitchStateToggle : SubDeviceState
{
    [JsonProperty("power")]
    public PowerCapability? Power { get; set; }
}