using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EWeLink.Cube.Api.Models.Devices;

public class DeviceContractResolver(ApiVersion apiVersion) : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
        if (typeof(SubDevice).IsAssignableFrom(type))
        {
            SubDevice subDevice = (SubDevice)Activator.CreateInstance(type)!;
            var addProperties = subDevice.AddPropertyList(apiVersion);
            properties = properties.Where(p => addProperties.Contains(p.UnderlyingName ?? string.Empty)).ToList();
        }
        if (typeof(SubDeviceCapability).IsAssignableFrom(type))
        {
            SubDeviceCapability capability = (SubDeviceCapability)Activator.CreateInstance(type)!;
            var addProperties = capability.AddPropertyList(apiVersion);
            properties = properties.Where(p => addProperties.Contains(p.UnderlyingName ?? string.Empty)).ToList();
        }
        
        return properties;
    }
}