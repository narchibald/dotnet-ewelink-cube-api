using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models;

public enum SoundResource
{
    [EnumMember(Value = "alert1")]
    Alert1,
    
    [EnumMember(Value = "alert2")]
    Alert2,
    
    [EnumMember(Value = "alert3")]
    Alert3,
    
    [EnumMember(Value = "alert4")]
    Alert4,
    
    [EnumMember(Value = "alert5")]
    Alert5,
    
    [EnumMember(Value = "doorbell1")]
    Doorbell1,
    
    [EnumMember(Value = "doorbell2")]
    Doorbell2,
    
    [EnumMember(Value = "doorbell3")]
    Doorbell3,
    
    [EnumMember(Value = "doorbell4")]
    Doorbell4,
    
    [EnumMember(Value = "doorbell5")]
    Doorbell5,
    
    [EnumMember(Value = "alarm1")]
    Alarm1,
    
    [EnumMember(Value = "alarm2")]
    Alarm2,
    
    [EnumMember(Value = "alarm3")]
    Alarm3,
    
    [EnumMember(Value = "alarm4")]
    Alarm4,
    
    [EnumMember(Value = "alarm5")]
    Alarm5,
}