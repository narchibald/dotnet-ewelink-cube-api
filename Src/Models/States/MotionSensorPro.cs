using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class MotionSensorPro : MotionSensor
{
    [JsonProperty("illumination-level")]
    public IlluminationLevelCapability? IlluminationLevel { get; set; }
    
    [JsonProperty("identify")]
    public IdentifyCapability? Identify { get; set; }
}