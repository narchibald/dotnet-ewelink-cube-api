using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class ButtonState : SubDeviceState
{
    [JsonProperty("battery")]
    public BatteryCapability? Battery { get; set; }
    
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
    
    [JsonProperty("press")]
    public PressCapability? Press { get; set; }
}