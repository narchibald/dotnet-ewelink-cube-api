using System;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models;

public class BeepObject
{
    public BeepObject(string name, int volume)
    {
        Name = name;
        if (volume is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(volume), volume, "Volume must be between 0 and 100.");
        Volume = volume;
    }

    [JsonProperty("name")]
    public string Name { get; }

    [JsonProperty("volume")]
    public int Volume { get; }
}