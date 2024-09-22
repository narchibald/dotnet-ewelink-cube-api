using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Devices;

public enum SubDeviceProtocol
{
    [EnumMember(Value = null)]
    None,
    
    [EnumMember(Value = "zigbee")]
    Zigbee,
    
    [EnumMember(Value = "onvif")]
    Onvif,
    
    [EnumMember(Value = "rtsp")]
    Rtsp,
    
    [EnumMember(Value = "esp32-cam")]
    Esp32Cam,
}