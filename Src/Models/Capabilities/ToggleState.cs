using System;
using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class ToggleState
{
    [JsonProperty("toggleState")]
    public SwitchState State { get; set; }
    
    [JsonProperty("updated_at")]
    [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
    public DateTimeOffset? UpdatedAt { get; set; }
}