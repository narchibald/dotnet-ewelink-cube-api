using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EWeLink.Cube.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EWeLink.Cube.Api;

internal class Screen(ILinkControl control) : IScreen
{
    public async Task<bool> SetBrightness(ScreenBrightnessMode mode, int? value = null)
    {
        control.EnsureAccessToken();
        
        try
        {
            await control.MakeRequest<Link.EmptyData>("screen/brightness", HttpMethod.Put, new ScreenBrightness(mode, value));
        }
        catch (RequestException ex)
        {
            return false;
        }

        return true;
    }
    
    public async Task<bool> SetDisplay(bool autoEnabled, int? duration = null)
    {
        control.EnsureAccessToken();

        try
        {
            await control.MakeRequest<Link.EmptyData>("screen/display", HttpMethod.Put, new ScreenDisplay(autoEnabled, duration));
        }
        catch (RequestException ex)
        {
            return false;
        }

        return true;
    }

    private class ScreenBrightness
    {
        public ScreenBrightness(ScreenBrightnessMode mode, int? value = null)
        {
            if (mode == ScreenBrightnessMode.Manual && value is null or < 0 or > 100)
                throw new ArgumentOutOfRangeException(nameof(value), value, "When the Brightness Mode is Manual a Value must given and be between 0 and 100.");
            
            Mode = mode;
            Value = mode == ScreenBrightnessMode.Manual ? value : null;
        }
        
        [JsonProperty("mode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ScreenBrightnessMode Mode { get; }
        
        [JsonProperty("value")]
        public int? Value { get; }
    }

    private class ScreenDisplay(bool autoEnabled, int? duration = null)
    {
        [JsonProperty("auto_screen_off")]
        public ScreenOffObject AutoScreenOff { get; } = new (autoEnabled, duration);
        
        public class ScreenOffObject
        {
            public ScreenOffObject(bool autoEnabled, int? duration = null)
            {
                if (autoEnabled && duration is null or < 15 or > 1800)
                    throw new ArgumentOutOfRangeException(nameof(duration), duration, "When Auto Screen Off is enable the a Value must be given and be between 15 and 1800.");
                
                Enable = autoEnabled;
                Duration = autoEnabled ? duration : null;
            }
            
            [JsonProperty("enable")]
            public bool Enable { get; }
            
            [JsonProperty("duration")]
            public int? Duration { get; }
        }
    }
}