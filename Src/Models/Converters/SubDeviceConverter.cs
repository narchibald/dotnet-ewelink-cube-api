using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EWeLink.Cube.Api.Models.Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EWeLink.Cube.Api.Models.Converters
{
    public class SubDeviceConverter : JsonConverter
    {
        private static readonly Dictionary<(string Model, string? Protocol, string? Category), Type> SubDeviceTypes;

        [ThreadStatic]
        private static bool disabled;

        static SubDeviceConverter()
        {
            SubDeviceTypes = typeof(SubDeviceConverter).Assembly.ExportedTypes
                .SelectMany(x => x.GetCustomAttributes<SubDeviceIdentifierAttribute>().Select(a => new { Attribute = a, Type = x }))
                .Where(x => x.Attribute != null).ToDictionary(x => (x.Attribute!.Model.ToLowerInvariant(), x.Attribute!.Protocol, x.Attribute!.DisplayCategory), v => v.Type);
        }

        // Disables the converter in a thread-safe manner.

        /// <inheritdoc/>
        public override bool CanWrite => false;

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
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        /// <inheritdoc/>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var deviceModel = jsonObject.Value<string>("model");
            var deviceCategory = string.IsNullOrWhiteSpace(deviceModel) ? jsonObject.Value<string>("display_category") : null;
            var deviceProtocol = jsonObject.Value<string>("protocol");
            var key = (deviceModel?.ToLowerInvariant() ?? string.Empty, deviceProtocol, deviceCategory);
            if (deviceModel is null || !SubDeviceTypes.TryGetValue(key, out var deviceType))
            {
                deviceType = typeof(GenericSubDevice);
            }

            try
            {
                Disabled = true;
                return jsonObject.ToObject(deviceType);
            }
            finally
            {
                Disabled = false;
            }
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
