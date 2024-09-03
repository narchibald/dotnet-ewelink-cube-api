using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class CameraStreamCapability
{
    [JsonProperty("configuration")]
    public CameraStreamConfiguration Configuration { get; set; }

    public class CameraStreamConfiguration
    {
        [JsonProperty("streamUrl")]
        public string StreamUrl { get; set; }
    }
}