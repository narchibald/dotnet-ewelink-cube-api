using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models;

public enum BeepResource
{
    [EnumMember(Value = "bootComplete")]
    BootComplete,
    
    [EnumMember(Value = "networkConnected")]
    NetworkConnected,
    
    [EnumMember(Value = "networkDisconnected")]
    NetworkDisconnected,
    
    [EnumMember(Value = "systemShutdown")]
    SystemShutdown,
    
    [EnumMember(Value = "deviceDiscovered")]
    DeviceDiscovered,
    
    [EnumMember(Value = "systemArmed")]
    SystemArmed,
    
    [EnumMember(Value = "systemDisarmed")]
    SystemDisarmed,
    
    [EnumMember(Value = "factoryReset")]
    FactoryReset  = 7,
}