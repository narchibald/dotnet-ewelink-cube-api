using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models;

public class PlayBeep(BeepResource resource, int volume) : SpeakerControl
{
    [JsonProperty("type")]
    public override SpeakerControlType Type => SpeakerControlType.PlayBeep;
    
    [JsonProperty("beep")]
    public BeepObject Beep { get; } = new(GetResourceValue(resource), volume);
}