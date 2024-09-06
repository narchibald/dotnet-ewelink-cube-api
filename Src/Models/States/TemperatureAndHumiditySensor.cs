using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class TemperatureAndHumiditySensor : SubDeviceState
{
    [JsonProperty("battery")]
    public BatteryCapability? Battery { get; set; }
    
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
    
    [JsonProperty("temperature")]
    public TemperatureCapability? Temperature { get; set; }
    
    [JsonProperty("thermostat-mode-detect")]
    public ThermostatModeDetectCapability? ThermostatModeDetect { get; set; }
    
    [JsonProperty("humidity")]
    public HumidityCapability? Humidity { get; set; }
    
    [JsonProperty("identify")]
    public IdentifyCapability? Identify { get; set; }
} 