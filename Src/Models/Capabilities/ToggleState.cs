using System;
using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class ToggleState : IUpdatable
{
    [JsonProperty("toggleState")]
    public SwitchState State { get; set; }

    [JsonProperty("updated_at")]
    [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
    public DateTimeOffset? UpdatedAt { get; set; }

    public void Update(object? data)
    {
        if (data is null)
            return;
        
        if (data is ToggleState updateData)
        {
            if (updateData.State != this.State)
            {
                this.State = updateData.State;
                if (updateData.UpdatedAt is null)
                    updateData.UpdatedAt = DateTimeOffset.UtcNow;
                this.UpdatedAt = updateData.UpdatedAt;
            }
        }
    }
}