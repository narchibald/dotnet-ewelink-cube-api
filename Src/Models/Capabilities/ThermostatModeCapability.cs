namespace EWeLink.Cube.Api.Models.Capabilities;

using System;
using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

[Capability("mode", Name = "thermostatMode")]
public class ThermostatModeCapability : Capability
{
    [JsonProperty("thermostatMode")]
    public ThermostatMode Mode { get; set; } = new();
    
    public class ThermostatMode : ITimestampedValue<ThermostatModeState>
    {
        [JsonProperty("modeValue")]
        public ThermostatModeState Value { get; set; }
    
        [JsonProperty("updated_at")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}