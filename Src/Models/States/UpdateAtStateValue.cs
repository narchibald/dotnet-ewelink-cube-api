using System;
using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States
{
    public abstract class UpdateAtStateValue
    {
        [JsonProperty("updated_at")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}