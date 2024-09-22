namespace EWeLink.Cube.Api.Models;

using Newtonsoft.Json;

public class SecurityMode
{
    [JsonProperty("sid")]
    public int Id  { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonProperty("enable")]
    public bool Enabled { get; set; }
}