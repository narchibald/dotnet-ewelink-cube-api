using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities;

public enum  IlluminationLevel
{
    [EnumMember(Value="darker")]
    Darker,
    
    [EnumMember(Value="brighter")]
    Brighter
}