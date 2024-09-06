using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class WindowDoorSensorPro : WindowDoorSensor
{
    [JsonProperty("tamper-alert")]
    public TamperAlertCapability? TamperAlert { get; set; }
}