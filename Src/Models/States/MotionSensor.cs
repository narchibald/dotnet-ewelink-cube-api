using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class MotionSensor : SubDeviceState
{
    [JsonProperty("battery")]
    public BatteryCapability? Battery { get; set; }
    
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
    
    [JsonProperty("detect")]
    public DetectCapability? Detect { get; set; }
}