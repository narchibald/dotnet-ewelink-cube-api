using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities;

public enum MotorControlState
{
    [EnumMember(Value = "close")]
    Close,
        
    [EnumMember(Value = "open")]
    Open,

    [EnumMember(Value = "stop")]
    Stop,
}