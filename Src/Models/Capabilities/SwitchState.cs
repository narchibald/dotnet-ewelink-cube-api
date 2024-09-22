using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities
{
    public enum SwitchState
    {
        [EnumMember(Value = "off")]
        Off,

        [EnumMember(Value = "on")]
        On,
    }
}