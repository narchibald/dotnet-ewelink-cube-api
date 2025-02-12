using System;

namespace EWeLink.Cube.Api.Models.Capabilities;

public interface ITimestampedValue<T>
{
    T Value { get; set; }
    
    DateTimeOffset? UpdatedAt { get; set; }
}