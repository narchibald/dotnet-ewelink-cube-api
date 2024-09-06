using EWeLink.Cube.Api.Models.Converters;
using EWeLink.Cube.Api.Models.States;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class HumidityCapability
{
    [JsonProperty("humidity")]
    [JsonConverter(typeof(IntToDecimalConverter), 0)]
    public decimal Value { get; set; }
}
