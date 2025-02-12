using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EWeLink.Cube.Api.Models.Capabilities.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EWeLink.Cube.Api.Models.Converters
{
    public class CapabilitySettingConverter : JsonConverter
    {
        private static readonly Dictionary<string, Type> SettingTypes;

        [ThreadStatic]
        private static bool disabled;

        static CapabilitySettingConverter()
        {
            SettingTypes = typeof(CapabilitySettingConverter).Assembly.ExportedTypes
                .SelectMany(x => x.GetCustomAttributes<CapabilitySettingAttribute>().Select(a => new { Attribute = a, Type = x }))
                .Where(x => x.Attribute != null).ToDictionary(x => x.Attribute!.Name, v => v.Type);
        }

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
                return;
            }
            
            var capabilitySettings = (List<CapabilitySetting>)value;
            if (capabilitySettings.Count == 0)
            {
                writer.WriteNull();
                return;
            }
            writer.WriteStartObject();
            foreach (var capabilitySetting in capabilitySettings)
            {
                writer.WritePropertyName(capabilitySetting.Name);
                serializer.Serialize(writer, capabilitySetting);
            }
            writer.WriteEndObject();
        }

        /// <inheritdoc/>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            List<CapabilitySetting> capabilitySettings = new ();
            foreach (var property in jsonObject.Properties())
            {
                var settingName = property.Name;
                var propertyValue = (JObject)property.Value;
                CapabilitySetting capabilitySetting;
                if (!SettingTypes.TryGetValue(settingName, out var settingType))
                {
                    var generic = new GenericCapabilitySetting();
                    JsonConvert.PopulateObject(propertyValue.ToString(), generic);
                    
                    capabilitySetting = generic;
                }
                else
                {
                    try
                    {
                        Disabled = true;
                        capabilitySetting = (CapabilitySetting)propertyValue.ToObject(settingType, serializer)!;
                    }
                    finally
                    {
                        Disabled = false;
                    }
                }
                capabilitySetting.Name = settingName;
                capabilitySettings.Add(capabilitySetting);
            }

            return capabilitySettings;
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(List<CapabilitySetting>).IsAssignableFrom(objectType);
        }
    }
}
