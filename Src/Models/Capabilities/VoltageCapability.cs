using System;
using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("voltage")]
public class VoltageCapability : Capability, ITimestampedValue<decimal>
{
    [JsonProperty("voltage")]
    [JsonConverter(typeof(IntToDecimalConverter), 2)]
    public decimal Value { get; set; }
    
    [JsonProperty("updated_at")]
    [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
    public DateTimeOffset? UpdatedAt { get; set; }

    /*public override void Update(Capability data)
    {
        bool updateTimeStamp = false;
        if (data is VoltageCapability capability)
        {
            updateTimeStamp = capability.UpdatedAt is null && capability.Value != this.Value;
        }
        
        base.Update(data);
        
        if (updateTimeStamp)
            this.UpdatedAt = DateTimeOffset.UtcNow;
    }*/
}