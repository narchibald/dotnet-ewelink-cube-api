using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using EWeLink.Cube.Api.Models.Capabilities.Settings;
using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Devices
{
    [Flags]
    public enum Permission : short
    {
        None = 0,
        
        [EnumMember(Value = "read")]
        Query = 1 << 0,
        
        Configure = 1 << 1,

        [EnumMember(Value = "write")]
        Update = 1 << 3,
        
        Updated = 1 << 2,

        [EnumMember(Value = "readWrite")]
        UpdateQuery = Update | Query,
        
        QueryConfigure = Query | Configure,
        
        UpdateUpdatedConfigure = Update | Updated | Configure,
        
        UpdateUpdated = Update | Updated,
        
        UpdatedConfigure = Updated | Configure,
        
        UpdateUpdatedQuery = Query | Updated | Configure,
    }

    public class SubDeviceCapability
    {
        [JsonProperty("capability")]
        public string Capability { get; set; } = string.Empty;

        [JsonProperty("permission")]
        public Permission Permission { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("configuration")]
        public Dictionary<string, object>? Configuration { get; set; }
        
        [JsonProperty("settings")]
        [JsonConverter(typeof(CapabilitySettingConverter))]
        public List<CapabilitySetting>? Settings { get; set; }

        internal virtual ISet<string> AddPropertyList(ApiVersion apiVersion) =>
            new HashSet<string>
            {
                nameof(Capability), nameof(Permission),
                apiVersion == ApiVersion.v1 ? nameof(Configuration) : nameof(Settings)
            };
    }
}