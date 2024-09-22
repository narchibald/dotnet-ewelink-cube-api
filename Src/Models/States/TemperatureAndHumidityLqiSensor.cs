using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class TemperatureAndHumidityLqiSensor : TemperatureAndHumiditySensor
{
    [JsonProperty("lqi")]
    public LqiCapability? Lqi { get; set; }
} 