using System;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class CameraStreamCapability
{
    [JsonProperty("configuration")]
    public CameraStreamConfiguration Configuration { get; set; } = null!;

    public class CameraStreamConfiguration
    {
        [JsonProperty("streamUrl")]
        public string StreamUrl { get; set; } = string.Empty;
    }
}