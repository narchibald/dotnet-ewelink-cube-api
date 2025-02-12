using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

[CapabilitySetting("percentageRange")]
public class PercentageRange : RangeSettingProperties
{
    [JsonProperty("step")]
    public decimal Step { get; set; }
}