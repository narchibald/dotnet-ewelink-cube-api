using System;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

[Capability("camera-stream")]
public class CameraStreamCapability : Capability
{
    [JsonProperty("configuration")]
    public CameraStreamConfiguration Configuration { get; set; } = null!;

    public class CameraStreamConfiguration
    {
        [JsonProperty("streamUrl")]
        public string StreamUrl { get; set; } = string.Empty;
    }
}