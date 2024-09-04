using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EWeLink.Cube.Api.Models;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api;

internal class Hardware(ILinkControl control) : IHardware
{
    public async Task<bool> Reboot()
    {
        control.EnsureAccessToken();

        try
        {
            await control.MakeRequest<Link.EmptyData>("hardware/reboot", HttpMethod.Post);
        }
        catch (RequestException ex)
        {
            return false;
        }

        return true;
    }

    public Task<bool> PlaySound(PlaySound sound)
        => InvokePlaySound(sound);

    public Task<bool> PlaySound(PlayBeep beep)
        => InvokePlaySound(beep);
    
    private async Task<bool> InvokePlaySound<T>(T sound)
        where T : SpeakerControl
    {
        control.EnsureAccessToken();

        try
        {
            await control.MakeRequest<Link.EmptyData>("hardware/speaker", HttpMethod.Post, sound);
        }
        catch (RequestException ex)
        {
            return false;
        }
        return true;
    }
}