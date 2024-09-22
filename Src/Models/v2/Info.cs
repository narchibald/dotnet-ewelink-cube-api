using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.v2;

public class Info : EWeLink.Cube.Api.Models.v1.Info
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}

