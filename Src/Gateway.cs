using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EWeLink.Cube.Api.Models;
using EWeLink.Cube.Api.Models.v1;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api;

internal class Gateway(ILinkControl control) : IGateway
{
    public Task<Status> GetStatus()
    {
        control.EnsureAccessToken();

        return control.MakeRequest<Status>("bridge/runtime");
    }

    public Task<Info> GetInfo()
    {
        control.EnsureAccessToken();
            
        return control.MakeRequest<Info>("bridge");
    }

    public async Task<bool> SetConfig(GatewayConfig config)
    {
        control.EnsureAccessToken();

        try
        {
            await control.MakeRequest<Link.EmptyData>("bridge/config", HttpMethod.Put, config);
        }
        catch (RequestException)
        {
            return false;
        }
        return true;
    }
}