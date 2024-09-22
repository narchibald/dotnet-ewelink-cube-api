using System;
using System.Linq;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Converters
{
    public class IntToDecimalConverter(int decimalPlaces = 2) : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanWrite => true;

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
            }
            else if (value is decimal decimalValue)
            {
                if (decimalValue == 0)
                    writer.WriteValue((int) decimalValue);
                else 
                    writer.WriteValue(int.Parse(decimalValue.ToString($"F{decimalPlaces}").Replace(".", string.Empty)));
            }
            else
            {
                writer.WriteNull();
            }
        }

        /// <inheritdoc/>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var nullableType = Nullable.GetUnderlyingType(objectType);
            switch (reader.Value)
            {
                case long longValue:
                    return (decimal)longValue;
                case int intValue:
                    return (decimal)intValue;
                case short shortValue:
                    return (decimal)shortValue;
            }

            string? val = reader.Value as string;
            if (string.IsNullOrEmpty(val))
            {
                if (nullableType == null)
                {
                    return Activator.CreateInstance(objectType);
                }

                return null;
            }
            
            var value = decimal.Parse(new string(val.Take(val!.Length - decimalPlaces).Concat(new[] { '.' }).Concat(val.Skip(val.Length - decimalPlaces)).ToArray()));
            return Convert.ChangeType(value, nullableType ?? objectType);
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal) || objectType == typeof(decimal?);
        }
    }
}