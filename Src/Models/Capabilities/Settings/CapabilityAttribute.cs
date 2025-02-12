using System;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

[AttributeUsage(AttributeTargets.Class)]
public class CapabilitySettingAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}