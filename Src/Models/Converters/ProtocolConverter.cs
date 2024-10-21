namespace EWeLink.Cube.Api.Models.Converters;

using System;
using Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class ProtocolConverter : StringEnumConverter
{
    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        Enum e = (Enum)value;
        value = e.GetEnumMemberValue()?.ToUpperInvariant();
        writer.WriteValue(value);
    }
}
