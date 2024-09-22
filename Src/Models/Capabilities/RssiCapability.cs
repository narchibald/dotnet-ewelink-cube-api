using EWeLink.Cube.Api.Models.States;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("rssi")]
public class RssiCapability : Capability
{
    [JsonProperty("rssi")]
    public int State { get; set; }
}