using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class SwvState : SubDeviceState
{
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
    
    [JsonProperty("power")]
    public PowerCapability? Power { get; set; }
}