using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

[CapabilitySetting("cycleCountRange")]
public class CycleCountRange : RangeSettingProperties
{
    [JsonProperty("step")]
    public decimal Step { get; set; }
}