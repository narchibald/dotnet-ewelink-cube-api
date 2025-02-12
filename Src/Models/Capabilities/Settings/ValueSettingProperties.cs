using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

public abstract class ValueSettingProperties<T> : CommonSettingProperties
{
    [JsonProperty("value")]
    public T Value { get; set; }
}