using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EWeLink.Cube.Api.Models;

public abstract class SpeakerControl
{
    [JsonProperty("type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public abstract SpeakerControlType Type { get; }
    
    protected static string GetResourceValue(Enum enumValue)
    {
        var type = enumValue.GetType();
        var memInfo = type.GetMember(enumValue.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);
        return ((EnumMemberAttribute)attributes[0]).Value!;
    }
}

public enum SpeakerControlType
{
    [EnumMember(Value = "play_sound")]
    PlaySound,
    
    [EnumMember(Value = "play_beep")]
    PlayBeep,
}