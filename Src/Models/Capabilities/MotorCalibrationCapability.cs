using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class MotorCalibrationCapability
{
    [JsonProperty("motorControl")]
    public MotorCalibrationState Mode { get; set; }
}