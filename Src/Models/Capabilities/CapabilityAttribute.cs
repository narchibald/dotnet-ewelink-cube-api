namespace EWeLink.Cube.Api.Models.Capabilities;

using System;

[AttributeUsage(AttributeTargets.Class)]
public class CapabilityAttribute(string capability) : Attribute
{
    public string Capability { get; } = capability;
    
    public string? Name { get; set; } = null;
}