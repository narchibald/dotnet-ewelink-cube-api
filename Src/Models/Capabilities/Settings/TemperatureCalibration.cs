using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

[CapabilitySetting("temperatureCalibration")]
public class TemperatureCalibration : RangeSettingProperties
{
    [JsonProperty("step")]
    public decimal Step { get; set; }
    
    [JsonProperty("value")]
    public decimal Value { get; set; }
}