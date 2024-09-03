using EWeLink.Cube.Api.Models.States;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities
{
    public class RssiCapability
    {
        [JsonProperty("rssi")]
        public int State { get; set; }
    }
}