using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models;

internal class Security(ILinkControl control) : ISecurity
{
    public async Task<IReadOnlyList<SecurityMode>> GetModes()
    {
        control.EnsureAccessToken();

        var data = await control.MakeRequest<SecurityList>("security");
        return data.Modes;
    }

    public async Task<bool> Enable()
    {
        control.EnsureAccessToken();

        try
        {
            await control.MakeRequest<Link.EmptyData>("security/enable", HttpMethod.Put);
        }
        catch (RequestException)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> Disable()
    {
        control.EnsureAccessToken();

        try
        {
            await control.MakeRequest<Link.EmptyData>("security/disable", HttpMethod.Put);
        }
        catch (RequestException)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> EnableMode(int sid)
    {
        control.EnsureAccessToken();

        try
        {
            await control.MakeRequest<Link.EmptyData>($"security/{sid}/enable", HttpMethod.Put);
        }
        catch (RequestException)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> DisableMode(int sid)
    {
        control.EnsureAccessToken();

        try
        {
            await control.MakeRequest<Link.EmptyData>($"security/{sid}/disable", HttpMethod.Put);
        }
        catch (RequestException)
        {
            return false;
        }
        return true;
    }

    private class SecurityList
    {
        [JsonProperty("security_list")]
        public List<SecurityMode> Modes { get; set; } = new();
    }
}