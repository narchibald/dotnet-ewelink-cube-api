using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EWeLink.Cube.Api.Models;

public class PlaySound(SoundResource resource, int volume, int countDown = 1) : SpeakerControl
{
    [JsonProperty("type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public override SpeakerControlType Type => SpeakerControlType.PlaySound;
    
    [JsonProperty("sound")]
    public SoundObject Sound { get; } = new(GetResourceValue(resource), volume, countDown);
}