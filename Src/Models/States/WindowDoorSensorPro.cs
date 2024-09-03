using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class WindowDoorSensorPro : WindowDoorSensor
{
    [JsonProperty("battery")]
    public BatteryCapability Battery { get; set; }
    
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
    
    [JsonProperty("tamper-alert")]
    public TamperAlertCapability TamperAlert { get; set; }
}