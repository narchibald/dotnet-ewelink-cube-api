using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities;

public enum MotionSensitivityState
{
    [EnumMember(Value = "low")]
    Low,
        
    [EnumMember(Value = "medium")]
    Medium,

    [EnumMember(Value = "high")]
    High,
}