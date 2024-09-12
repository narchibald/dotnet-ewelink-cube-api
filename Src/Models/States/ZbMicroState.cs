using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class ZbMicroState : SubDeviceState
{
    [JsonProperty("transmit-power")]
    public TransmitPowerCapability? TransmitPower { get; set; }
    
    [JsonProperty("startup")]
    public StartupCapability? Startup { get; set; }
    
    [JsonProperty("power")]
    public PowerCapability? Power { get; set; }
    
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
}