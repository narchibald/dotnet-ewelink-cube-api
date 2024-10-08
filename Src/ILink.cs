using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EWeLink.Cube.Api.Models.Capabilities;
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
    
    ISecurity Security { get; }

    Task<string?> GetAccessToken(CancellationToken? cancellationToken = default);

    Task<ISubDevice?> GetDevice(string serialNumber);
    
    Task<bool> SetSwitchState(string serialNumber, SwitchState state, Channel channel = Channel.One);
    
    Task<bool> SetPowerState(string serialNumber, SwitchState state);
    
    Task<bool> SetToggleState(string serialNumber, SwitchState state, Channel channel);
    
    Task<bool> SetPercentage(string serialNumber, int percent);
    
    Task<IReadOnlyList<ISubDevice>> GetDevices();
}

internal interface ILinkControl
{
    IPAddress IpAddress { get; }
    
    int Port { get; }
    
    ApiVersion ApiVersion { get; }
    
    string EnsureAccessToken();
    
    Task<T> MakeRequest<T>(string path, HttpMethod? method = null, object? content = null);
}