using System;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

public abstract class EnumSettingProperties<TE> : CommonSettingProperties
    where TE : Enum
{
    [JsonProperty("value")]
    public TE Value { get; set; }
    
    [JsonProperty("values")]
    public TE[] Values { get; set; }
}