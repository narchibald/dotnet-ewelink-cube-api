using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("motor-clb")]
public class MotorCalibrationCapability : Capability
{
    [JsonProperty("motorControl")]
    public MotorCalibrationState Mode { get; set; }
}