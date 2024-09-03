using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities;

public enum TemperatureModeState
{
    [EnumMember(Value = "COMFORT")]
    Comfort,

    [EnumMember(Value = "COLD")]
    Cold,
    
    [EnumMember(Value = "HOT")]
    Hot,
}