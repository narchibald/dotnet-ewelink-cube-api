using System;
using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("lqi")]
public class LqiCapability : Capability, ITimestampedValue<decimal>
{
    [JsonProperty("lqi")]
    [JsonConverter(typeof(IntToDecimalConverter), 0)]
    public decimal Value { get; set; }
    
    [JsonProperty("updated_at")]
    [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
    public DateTimeOffset? UpdatedAt { get; set; }
    
    /*public override void Update(Capability data)
    {
        bool updateTimeStamp = false;
        if (data is LqiCapability capability)
        {
            updateTimeStamp = capability.UpdatedAt is null && capability.Value != this.Value;
        }
        
        base.Update(data);
        
        if (updateTimeStamp)
            this.UpdatedAt = DateTimeOffset.UtcNow;
    }*/
}