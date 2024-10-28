using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class ZbMicroState : MicroState
{
    [JsonProperty("transmit-power")]
    public TransmitPowerCapability? TransmitPower { get; set; }
    
    [JsonProperty("startup")]
    public StartupCapability? Startup { get; set; }
    
    [JsonProperty("lqi")]
    public LqiCapability? Lqi { get; set; }
}