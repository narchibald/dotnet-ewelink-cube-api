using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
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
    }
}