using System;

namespace EWeLink.Cube.Api.Models.Devices
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class SubDeviceIdentifierAttribute(string model) : Attribute
    {
        public string Model { get; } = model;

        public string? Protocol { get; set; } = "zigbee";
        
        public string? DisplayCategory { get; set; }
    }
}
