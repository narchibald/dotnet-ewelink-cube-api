using System.Reflection;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

public abstract class CapabilitySetting
{
    protected CapabilitySetting()
    {
        Name = this.GetType().GetCustomAttribute<CapabilitySettingAttribute>()?.Name ?? string.Empty;
    }

    [JsonIgnore]
    public string Name { get; set; } = string.Empty;
}