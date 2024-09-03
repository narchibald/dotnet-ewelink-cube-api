using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models
{
    public enum CapabilityPermission
    {
        [EnumMember(Value = "read")]
        Read = 1,

        [EnumMember(Value = "write")]
        Write = 2,

        [EnumMember(Value = "readWrite")]
        ReadWrite = Read | Write,
    }

    public class SubDeviceCapability
    {
        [JsonProperty("capability")]
        public string Capability { get; set; }

        [JsonProperty("permission")]
        public CapabilityPermission Permission { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("configuration")]
        public Dictionary<string, object>? Configuration { get; set; }
    }
}