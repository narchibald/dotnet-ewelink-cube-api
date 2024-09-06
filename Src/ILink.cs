using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EWeLink.Cube.Api.Models.Devices;

namespace EWeLink.Cube.Api;

public interface ILink : IDisposable
{
    IPAddress IpAddress { get; }
    
    int Port { get; }
    
    string? AccessToken { get; }
    
    IGateway Gateway { get; }
    
    IHardware Hardware { get; }
    
    IScreen Screen { get; }

    Task<string?> GetAccessToken(CancellationToken? cancellationToken = default);

    Task<ISubDevice?> GetDevice(string serialNumber);
    
    Task<IReadOnlyList<ISubDevice>> GetDevices();
    
    Task<IEventStream> GetEventStream();
}

internal interface ILinkControl
{
    IPAddress IpAddress { get; }
    
    int Port { get; }
    
    string EnsureAccessToken();
    
    Task<T> MakeRequest<T>(string path, HttpMethod? method = null, object? content = null);
}