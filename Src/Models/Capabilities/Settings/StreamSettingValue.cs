using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

public class StreamSettingValue
{
    [JsonProperty("stream_url")]
    public string StreamUrl { get; set; }
    
    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;
    
    [JsonProperty("password")]
    public string Password { get; set; } = string.Empty;
    
    [JsonProperty("videoCodec")]
    public string? VideoCodec { get; set; }
    
    [JsonProperty("audioCodec")]
    public string? AudioCodec { get; set; }
    
    [JsonProperty("resolution")]
    public StreamResolution? Resolution { get; set; }
    
    [JsonProperty("keyFrameInterval")]
    public decimal? KeyFrameInterval { get; set; }
    
    [JsonProperty("samplingRate")]
    public decimal? SamplingRate { get; set; }

    [JsonProperty("dataRate")]
    public decimal? DataRate { get; set; }

    public class StreamResolution
    {
        [JsonProperty("width")]
        public decimal Width { get; set; } = 1080;
        
        [JsonProperty("height")]
        public decimal Height { get; set; } = 720;
    }
}