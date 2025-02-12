using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class MiniState : SubDeviceState
{
    [JsonProperty("power")]
    public PowerCapability? Power { get; set; }
    
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
}