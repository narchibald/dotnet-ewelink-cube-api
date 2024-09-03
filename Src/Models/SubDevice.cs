using System.Collections.Generic;
using EWeLink.Cube.Api.Models.Converters;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models
{
    [JsonConverter(typeof(SubDeviceConverter))]
    public class SubDevice : ISubDevice
    {
        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }
        
        [JsonProperty("third_serial_number")]
        public string? ThirdPartySerialNumber { get; set; }
        
        [JsonProperty("service_address")]
        public string? ServiceAddress { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("firmware_version")]
        public string FirmwareVersion { get; set; }
        
        [JsonProperty("hostname")]
        public string? Hostname { get; set; }
        
        [JsonProperty("app_name")]
        public string? AppName { get; set; }

        [JsonProperty("display_category")]
        public string DisplayCategory { get; set; }

        [JsonProperty("capabilities")]
        public List<SubDeviceCapability> Capabilities { get; set; }

        [JsonProperty("protocol")]
        public SubDeviceProtocol Protocol { get; set; }

        [JsonProperty("tags")]
        public Dictionary<string, object> Tags { get; set; }

        [JsonProperty("online")]
        public bool Online { get; set; }
        
        [JsonProperty("subnet")]
        public bool? Subnet { get; set; }
    }
}