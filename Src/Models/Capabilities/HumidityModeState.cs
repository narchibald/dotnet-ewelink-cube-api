using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities;

public enum HumidityModeState
{
    [EnumMember(Value = "COMFORT")]
    Comfort,

    [EnumMember(Value = "DRY")]
    Dry,
    
    [EnumMember(Value = "WET")]
    Wet,
}