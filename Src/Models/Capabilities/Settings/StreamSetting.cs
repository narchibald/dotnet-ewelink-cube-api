namespace EWeLink.Cube.Api.Models.Capabilities.Settings;

[CapabilitySetting("streamSetting")]
public class StreamSetting : ObjectSettingsProperties<StreamSettingValue>
{
    public StreamSetting()
    {
        Value = new StreamSettingValue();
    }
}