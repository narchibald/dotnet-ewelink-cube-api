using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

[CapabilitySetting("timeRange")]
public class TimeRange : RangeSettingProperties
{
    [JsonProperty("unit")]
    public string Unit { get; set; }
}