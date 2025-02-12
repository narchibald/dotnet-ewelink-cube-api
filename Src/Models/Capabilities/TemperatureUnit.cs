using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities;

public enum TemperatureUnit
{
    [EnumMember(Value = "c")]
    Celsius,
    
    [EnumMember(Value = "f")]
    Fahrenheit,
}