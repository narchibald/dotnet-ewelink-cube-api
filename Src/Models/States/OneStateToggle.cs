using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States
{
    public class OneStateToggle : SwitchStateToggle
    {
        [JsonProperty("rssi")]
        public RssiCapability Rssi { get; set; }
    }
}