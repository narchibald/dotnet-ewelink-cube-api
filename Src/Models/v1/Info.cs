using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.v1;

public class Info
{
    [JsonProperty("ip")]
    public string IPAddress { get; set; } = string.Empty;
    
    [JsonProperty("mac")]
    public string MacAddress { get; set; } = string.Empty;
    
    [JsonProperty("domain")]
    public string Domain { get; set; } = string.Empty;
    
    [JsonProperty("fw_version")]
    public string FirmwareVersion { get; set; } = string.Empty;
}

