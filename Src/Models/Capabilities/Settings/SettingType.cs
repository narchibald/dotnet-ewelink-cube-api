using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

public enum SettingType
{
    [EnumMember(Value = "numeric")]
    Numeric,
    
    [EnumMember(Value = "enum")]
    Enum,
    
    [EnumMember(Value = "object")]
    Object,
    
    [EnumMember(Value = "boolean")]
    Boolean,
}