using System.Collections.Generic;
using EWeLink.Cube.Api.Models.Converters;
using EWeLink.Cube.Api.Models.States;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Devices
{
    [JsonConverter(typeof(SubDeviceConverter))]
    public interface ISubDevice
    {
        string SerialNumber { get; set; }
        
        string? ThirdPartySerialNumber { get; set; }
        
        string? ServiceAddress { get; set; }

        string Name { get; set; }

        string Manufacturer { get; set; }

        string Model { get; set; }

        string FirmwareVersion { get; set; }
        
        string? Hostname { get; set; }
        
        string? AppName { get; set; }

        string DisplayCategory { get; set; }

        List<SubDeviceCapability> Capabilities { get; set; }

        SubDeviceProtocol Protocol { get; set; }

        Dictionary<string, object> Tags { get; set; }

        bool Online { get; set; }
        
        bool? Subnet { get; set; }
    }

    public interface ISubDevice<out T> : ISubDevice
        where T : SubDeviceState
    {
        public T State { get; }
    }

    public class SubDevice<T> : SubDevice, ISubDevice<T>
        where T : SubDeviceState
    {
        [JsonProperty("state")]
        public T State { get; set; } = null!;
    }
}