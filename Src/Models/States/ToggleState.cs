using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States
{
    public class ToggleState : UpdateAtStateValue
    {
        [JsonProperty("toggleState")]
        public SwitchState State { get; set; }
    }
}