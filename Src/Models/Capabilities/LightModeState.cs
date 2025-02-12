using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities
{
    public enum LightModeState
    {
        [EnumMember(Value = "colorTemperature")]
        ColorTemperature,

        [EnumMember(Value = "color")]
        Color,
        
        [EnumMember(Value = "whiteLight")]
        WhiteLight,
    }
}