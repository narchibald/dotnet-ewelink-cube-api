using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities
{
    public enum StartupState
    {
        [EnumMember(Value = "stay")]
        Stay,
        
        [EnumMember(Value = "on")]
        On,

        [EnumMember(Value = "off")]
        Off,
    }
}