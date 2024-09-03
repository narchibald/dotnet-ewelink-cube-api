using EWeLink.Cube.Api.Models.Converters;
using EWeLink.Cube.Api.Models.States;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class TransmitPowerCapability
{
    [JsonProperty("transmitPower")]
    public int Power { get; set; }
}