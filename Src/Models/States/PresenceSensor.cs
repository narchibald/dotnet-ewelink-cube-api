using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class PresenceSensor : SubDeviceState
{
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
    
    [JsonProperty("detected")]
    public DetectCapability Detect { get; set; }
    
    [JsonProperty("illumination-level")]
    public IlluminationLevelCapability IlluminationLevel { get; set; }
}