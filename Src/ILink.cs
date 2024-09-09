using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EWeLink.Cube.Api.Models.Devices;
using EWeLink.Cube.Api.Models.States;

namespace EWeLink.Cube.Api;

public interface ILink : IDisposable
{
    event Action<ILink, ILinkEvent<SubDeviceState>>? DeviceStateUpdated;
    
    IPAddress IpAddress { get; }
    
    int Port { get; }
    
    string? AccessToken { get; }
    
    IGateway Gateway { get; }
    
    IHardware Hardware { get; }
    
    IScreen Screen { get; }

    Task<string?> GetAccessToken(CancellationToken? cancellationToken = default);

    Task<ISubDevice?> GetDevice(string serialNumber);
    
    Task<IReadOnlyList<ISubDevice>> GetDevices();
}

internal interface ILinkControl
{
    IPAddress IpAddress { get; }
    
    int Port { get; }
    
    string EnsureAccessToken();
    
    Task<T> MakeRequest<T>(string path, HttpMethod? method = null, object? content = null);
}