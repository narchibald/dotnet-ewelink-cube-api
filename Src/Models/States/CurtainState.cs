using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class CurtainState : SubDeviceState
{
    [JsonProperty("battery")]
    public BatteryCapability? Battery { get; set; }
    
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
    
    [JsonProperty("motor-control")]
    public MotorControlCapability? MotorControl { get; set; }
    
    [JsonProperty("motor-reverse")]
    public MotorReverseCapability? MotorReverse { get; set; }
    
    [JsonProperty("motor-clb")]
    public MotorCalibrationCapability? MotorCalibration { get; set; }
    
    [JsonProperty("percentage")]
    public PercentageCapability? Percentage { get; set; }
}