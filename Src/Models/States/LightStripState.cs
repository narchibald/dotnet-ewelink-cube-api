namespace EWeLink.Cube.Api.Models.States;

using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

public class ListStripState : SwitchStateToggle
{
    [JsonProperty("brightness")]
    public BrightnessCapability? Brightness { get; set; }
    
    [JsonProperty("color-temperature")]
    public ColorTemperatureCapability? ColorTemperature { get; set; }
    
    [JsonProperty("color-rgb")]
    public ColorRgbCapability? ColorRgb { get; set; }
    
    [JsonProperty("mode")]
    public LightModeCapability? Mode { get; set; }
}