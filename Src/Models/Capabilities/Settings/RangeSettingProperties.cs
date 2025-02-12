using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

public abstract class RangeSettingProperties : CommonSettingProperties
{
    [JsonProperty("min")]
    public decimal Minimum { get; set; }
    
    [JsonProperty("max")]
    public decimal Maximum { get; set; }
}