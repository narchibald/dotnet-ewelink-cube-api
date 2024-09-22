using System;
using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("voltage")]
public class VoltageCapability : Capability
{
    [JsonProperty("voltage")]
    [JsonConverter(typeof(IntToDecimalConverter), 2)]
    public decimal Value { get; set; }
    
    [JsonProperty("updated_at")]
    [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
    public DateTimeOffset? UpdatedAt { get; set; }
}