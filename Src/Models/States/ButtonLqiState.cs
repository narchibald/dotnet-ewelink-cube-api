using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States;

public class ButtonLqiState : ButtonState
{
    [JsonProperty("lqi")]
    public LqiCapability? Lqi { get; set; }
}