using System.Collections.Generic;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States
{
    public class TwoStateToggle : SubDeviceState
    {
        [JsonProperty("toggle")]
        public Dictionary<string, ToggleState> Toggle { get; set; } = new();
    }
}