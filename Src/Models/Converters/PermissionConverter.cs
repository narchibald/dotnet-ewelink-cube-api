using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EWeLink.Cube.Api.Extensions;
using EWeLink.Cube.Api.Models.Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EWeLink.Cube.Api.Models.Converters
{
    public class PermissionConverter(ApiVersion apiVersion) : JsonConverter
    {
        [ThreadStatic]
        private static bool disabled;

        // Disables the converter in a thread-safe manner.

        /// <inheritdoc/>
        public override bool CanWrite => true;

        /// <inheritdoc/>
        public override bool CanRead => !this.Disabled;

        private bool Disabled
        {
            get => disabled;
            set => disabled = value;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
            }
            else if (value is Permission permission)
            {
                string output;
                if (apiVersion == ApiVersion.v2)
                {
                    output = Convert.ToString((short)permission, 2).PadLeft(4, '0');
                }
                else
                {
                    output = permission.GetEnumMemberValue()!;
                }

                writer.WriteValue(output);
            }

            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            string? val = reader.Value as string;
            if (val is null || string.IsNullOrEmpty(val))
                return Permission.None;
            
            if (apiVersion == ApiVersion.v2)
                return (Permission)Convert.ToInt16(val, 2);
            
            return val.ParseFromEnumMemberValue<Permission>();
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Permission);
        }
    }
}
