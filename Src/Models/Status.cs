using System;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models;

public class Status
{
    [JsonProperty("ram_used")]
    public int RamUsed { get; set; }
    
    [JsonProperty("cpu_used")]
    public int CpuUsed { get; set; }
    
    [JsonProperty("power_up_time")]
    public DateTimeOffset PowerUpTime { get; set; }
}