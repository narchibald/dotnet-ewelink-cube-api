using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

[CapabilitySetting("timeZoneOffset")]
public class TimeZoneOffset : RangeSettingProperties
{
    [JsonProperty("value")]
    public decimal Value { get; set; }
}