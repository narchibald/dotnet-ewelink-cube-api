using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities
{
    public enum TamperState
    {
        [EnumMember(Value = "detected")]
        Detected,

        [EnumMember(Value = "clear")]
        Clear,
    }
}