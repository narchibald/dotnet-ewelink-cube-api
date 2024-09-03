using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities;

public enum PressState
{
    [EnumMember(Value = "singlePress")]
    SinglePress,
        
    [EnumMember(Value = "doublePress")]
    DoublePress,

    [EnumMember(Value = "longPress")]
    LongPress,
}