namespace EWeLink.Cube.Api.Models.Capabilities;

using System.Runtime.Serialization;

public enum ThermostatModeState
{
    [EnumMember(Value = "auto")]
    Auto,

    [EnumMember(Value = "manual")]
    Manual,
}