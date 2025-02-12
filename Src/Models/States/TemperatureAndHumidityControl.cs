using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class TemperatureAndHumidityControl : SubDeviceState
{
    [JsonProperty("power")]
    public PowerCapability? Power { get; set; }
    
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
    
    [JsonProperty("temperature")]
    public TemperatureCapability? Temperature { get; set; }
    
    [JsonProperty("mode")]
    public ThermostatModeCapability? ThermostatMode { get; set; }
    
    [JsonProperty("humidity")]
    public HumidityCapability? Humidity { get; set; }
} 