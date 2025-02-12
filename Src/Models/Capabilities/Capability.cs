using System;
using System.Linq;
using System.Reflection;

namespace EWeLink.Cube.Api.Models.Capabilities;

public class Capability
{
    public void Update(Capability data)
    {
        var type = this.GetType();
        if (this.GetType() != data.GetType())
            return;
        
        var capabilityProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop is { CanRead: true, CanWrite: true }) // Check compatibility
            .ToList();

        bool isTimestampedValue = type
            .GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITimestampedValue<>));

        PropertyInfo? timestampedValueProperty = isTimestampedValue ? capabilityProperties.First(x => x.Name == "UpdatedAt") : null;

        foreach (var property in capabilityProperties)
        {
            var newValue = property.GetValue(data);
            if (typeof(IUpdatable).IsAssignableFrom(property.PropertyType))
            {
                if (newValue is null)
                    continue;
                
                var currentValue = property.GetValue(this);
                if (currentValue is null)
                {
                    currentValue = Activator.CreateInstance(property.PropertyType)!;
                    property.SetValue(this, currentValue);
                }
                
                var updatable = (IUpdatable)currentValue;
                updatable.Update(property.GetValue(data));
                continue;
            }

            if (isTimestampedValue)
            {
                if (property.Name == "Value")
                {
                    var currentValue = property.GetValue(this);
                    if (currentValue == newValue)
                        timestampedValueProperty = null;
                }
                else if (property.Name == "UpdatedAt" && newValue is not null)
                    timestampedValueProperty = null;
            }
            
            property.SetValue(this, newValue);
        }

        if (timestampedValueProperty is not null)
        {
            timestampedValueProperty.SetValue(this, DateTimeOffset.UtcNow);
            timestampedValueProperty.SetValue(data, DateTimeOffset.UtcNow);
        }
    }
}