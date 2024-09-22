using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class PowerMeterSwitch : SwitchStateToggle
{
    [JsonProperty("electric-power")]
    public ElectricPowerCapability? ElectricPower { get; set; }
    
    [JsonProperty("voltage")]
    public VoltageCapability? Voltage { get; set; }
    
    [JsonProperty("electric-current")]
    public ElectricCurrentCapability? ElectricCurrent { get; set; }
    
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
}