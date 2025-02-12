using EWeLink.Cube.Api.Models.Devices;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

public abstract class CommonSettingProperties : CapabilitySetting
{
    [JsonProperty("type")]
    public SettingType Type { get; set; }
    
    [JsonProperty("permission")]
    public Permission Permission { get; set; }
}