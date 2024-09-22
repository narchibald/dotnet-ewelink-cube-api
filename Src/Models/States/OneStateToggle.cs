using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class OneStateToggle : SwitchStateToggle
{
    [JsonProperty("toggle")]
    public OneToggleCapability? Toggle { get; set; }
    
    [JsonProperty("rssi")]
    public RssiCapability? Rssi { get; set; }
}