using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models;

public enum ScreenBrightnessMode
{
    [EnumMember(Value = "auto")]
    Auto,
    
    [EnumMember(Value = "manual")]
    Manual,
}