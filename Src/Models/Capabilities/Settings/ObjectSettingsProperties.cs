using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

public abstract class ObjectSettingsProperties<TO> : CommonSettingProperties
{
    protected ObjectSettingsProperties()
    {
        Type = SettingType.Object;
    }
    
    [JsonProperty("value")]
    public TO Value { get; set; }
}