using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EWeLink.Cube.Api.Models.Capabilities;
using EWeLink.Cube.Api.Models.Devices;

namespace EWeLink.Cube.Api.Extensions;

public static class CapabilityExtensions
{
    private static readonly Dictionary<Type, (string Capability, string? Name)> TypeToCapability;

    static CapabilityExtensions()
    {
        TypeToCapability = typeof(CapabilityExtensions).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Capability)) && x.GetCustomAttribute<CapabilityAttribute>() != null)
            .Select(x =>
            {
                var attribute = x.GetCustomAttribute<CapabilityAttribute>()!;
                return (x, (attribute.Capability, attribute.Name));
            })
            .ToDictionary(x => x.Item1, x => x.Item2);
    }
    
    public static bool HasCapability<T>(this ISubDevice device, Permission? requiredPermission = null)
        where T : Capability
    {
        if (!TypeToCapability.TryGetValue(typeof(T), out var value))
            return false;
        return device.Capabilities.Any(x => x.Capability == value.Capability && x.Name == value.Name && (requiredPermission is null || x.Permission.HasFlag(requiredPermission)));
    }
    
    public static string GetCapabilityName(this Capability capability) => TypeToCapability[capability.GetType()].Capability;
}