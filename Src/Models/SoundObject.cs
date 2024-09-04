using System;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models;

public class SoundObject
{
    public SoundObject(string name, int volume, int countDown)
    {
        Name = name;
        if (volume is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(volume), volume, "Volume must be between 0 and 100.");
        Volume = volume;
        if (countDown is < 0 or > 1799)
            throw new ArgumentOutOfRangeException(nameof(countDown), volume, "Count Down must be between 0 and 1799.");
        CountDown = countDown;
    }

    [JsonProperty("name")]
    public string Name { get; }

    [JsonProperty("volume")]
    public int Volume { get; }

    [JsonProperty("countdown")]
    public int CountDown { get; }
}