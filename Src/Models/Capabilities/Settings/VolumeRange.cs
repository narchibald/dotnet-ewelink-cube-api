using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

[CapabilitySetting("volumeRange")]
public class VolumeRange : RangeSettingProperties
{
    [JsonProperty("unit")]
    public string Unit { get; set; }
}