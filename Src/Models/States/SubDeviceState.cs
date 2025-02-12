using System;
using System.Linq;
using System.Reflection;
using EWeLink.Cube.Api.Models.Capabilities;

namespace EWeLink.Cube.Api.Models.States
{
    public class SubDeviceState
    {
        public void Update(SubDeviceState data)
        {
            var type = this.GetType();
            if (this.GetType() != data.GetType())
                return;
            
            var capabilityProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => typeof(Capability).IsAssignableFrom(prop.PropertyType)) // Check compatibility
                .ToList();

            foreach (var prop in capabilityProperties)
            {
                Capability? currentValue = prop.GetValue(this) as Capability;
                Capability? newValue = prop.GetValue(data) as Capability;
                if ((currentValue is null && newValue is null) || newValue is null)
                    continue;

                if (currentValue is null)
                {
                    currentValue = (Capability)Activator.CreateInstance(prop.PropertyType)!;
                    prop.SetValue(this, currentValue);
                }

                currentValue.Update(newValue);
            }
        }
    }
}