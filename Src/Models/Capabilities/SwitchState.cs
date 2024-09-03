using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities
{
    public enum SwitchState
    {
        [EnumMember(Value = "on")]
        On,

        [EnumMember(Value = "off")]
        Off,
    }
}