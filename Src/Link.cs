using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using EWeLink.Cube.Api;
using EWeLink.Cube.Api.Models;
using EWeLink.Cube.Api.Extensions;
using EWeLink.Cube.Api.Models.Capabilities;
using EWeLink.Cube.Api.Models.Converters;
using EWeLink.Cube.Api.Models.Devices;
using EWeLink.Cube.Api.Models.States;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EWeLink.Cube.Api;

public class Link : ILink, ILinkControl
{
    private readonly string BasePathFormat = "open-api/v{0}/rest/";
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IDeviceCache deviceCache;
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<Link> logger;
    private EventStream? eventStream;
    private int port = 80;
    private bool portLocked = false;
    private ApiVersion apiVersion = ApiVersion.v2;
    private bool apiLocked = false;
    private bool initialized = false;
    private Action<ILink, ILinkEvent<SubDeviceState>>? deviceStateUpdatedEvent;

    public Link(IPAddress ipAddress, string? accessToken, int? port,  ApiVersion? apiVersion, IHttpClientFactory httpClientFactory, IDeviceCache deviceCache, ILoggerFactory loggerFactory)
    {
        this.IpAddress = ipAddress;
        if (port is not null)
        {
            this.port = port.Value;
            portLocked = true;
        }

        if (apiVersion is not null)
        {
            this.apiVersion = apiVersion.Value;
            this.apiLocked = true;
        }

        this.AccessToken = accessToken;
        this.httpClientFactory = httpClientFactory;
        this.deviceCache = deviceCache;
        this.loggerFactory = loggerFactory;
        this.logger = loggerFactory.CreateLogger<Link>();
        Gateway = new Gateway(this);
        Screen = new Screen(this);
        Hardware = new Hardware(this);
        Security = new Security(this);
    }

    public event Action<ILink, ILinkEvent<SubDeviceState>>? DeviceStateUpdated
    {
        add
        {
            if (deviceStateUpdatedEvent == null)
            {
                Task.Run(async () =>
                {
                    await StartEventStream();
                    this.deviceStateUpdatedEvent += value;
                });
            }
            else
            {
                this.deviceStateUpdatedEvent += value;
            }
        }
        remove
        {
            this.deviceStateUpdatedEvent -= value;
            if (deviceStateUpdatedEvent == null)
            {
                Task.Run(async () => await StopEventStream());
            }
        }
    }
        
    public IPAddress IpAddress { get; }

    public string? AccessToken { get; private set; }

    public IGateway Gateway { get; }
    
    public IHardware Hardware { get; }
    
    public IScreen Screen { get; }
    
    public ISecurity Security { get; }

    public int Port => port;
    
    public ApiVersion ApiVersion => apiVersion;
    
    private string BasePath => string.Format(BasePathFormat, (int)apiVersion);
    
    public async Task<string?> GetAccessToken(CancellationToken? cancellationToken = default)
    {
        CancellationTokenSource? cancellationTokenSource = null;
        if (cancellationToken is null)
        {
            cancellationTokenSource = new(TimeSpan.FromMinutes(5));
            cancellationToken = cancellationTokenSource.Token;
        }
        
        try
        {
            while (!cancellationToken.Value.IsCancellationRequested)
            {
                try
                {
                    var response = await MakeRequest<AccessTokenData>("bridge/access_token");
                    if (!string.IsNullOrEmpty(response.Token))
                        AccessToken = response.Token;
                    return response.Token;
                }
                catch (RequestException ex) when (ex.Error is (int)HttpStatusCode.Unauthorized)
                {
                    logger.LogDebug("Failed to get access token. reason: {Reason}", ex.Message);
                }

                await Task.Delay(500, cancellationToken.Value);
            }
        }
        finally
        {
            cancellationTokenSource?.Dispose();
        }

        return null;
    }
    
    public async Task<ISubDevice?> GetDevice(string serialNumber)
    {
        if (!deviceCache.TryGetDevice(serialNumber, out var device))
        {
            await GetDevices();
            deviceCache.TryGetDevice(serialNumber, out device);
        }
        
        return device;
    }

    public async Task<bool> SetSwitchState(string serialNumber, SwitchState state, Channel channel = Channel.One)
    {
        bool isPowerStateChange = false;
        _ = await GetDeviceForUpdate(serialNumber, (d) =>
        {
            void CheckUpdateCapability<T>() where T : Capability
            {
                if (!d.HasCapability<T>(Permission.Update))
                    throw new NotSupportedCapabilityException<T>();
            }

            switch (channel)
            {
                case Channel.One:
                {
                    if (d.HasCapability<OneToggleCapability>())
                        CheckUpdateCapability<OneToggleCapability>();
                    CheckUpdateCapability<PowerCapability>();
                    isPowerStateChange = true;
                }
                    break;
                case Channel.Two:
                    CheckUpdateCapability<TwoToggleCapability>();
                    break;
                case Channel.Three:
                    CheckUpdateCapability<ThreeToggleCapability>();
                    break;
            }
        });
        
        if(isPowerStateChange)
            return await SetPowerState(serialNumber, state);

        return await SetToggleState(serialNumber, state, channel);
    }

    public async Task<IReadOnlyList<ISubDevice>> GetDevices()
    {
        EnsureAccessToken();

        var deviceList = (await MakeRequest<Devices>("devices")).List;
        deviceCache.UpdateCache(deviceList);
        return deviceList;
    }

    public async Task<bool> SetPowerState(string serialNumber, SwitchState state)
    {
        _ = GetDeviceForUpdate<PowerCapability>(serialNumber);

        var update = new UpdateDeviceState(new PowerCapability { State = state });
        try
        {
            await MakeRequest<EmptyData>($"devices/{serialNumber}", HttpMethod.Put, update);
        }
        catch (RequestException ex) when(ex.Error is 110006)
        {
            return false;
        }
        
        return true;
    }

    public async Task<bool> SetToggleState(string serialNumber, SwitchState state, Channel channel)
    {
        _ = await (channel switch
        {
            Channel.One => GetDeviceForUpdate<OneToggleCapability>(serialNumber),
            Channel.Two => GetDeviceForUpdate<TwoToggleCapability>(serialNumber),
            Channel.Three => GetDeviceForUpdate<ThreeToggleCapability>(serialNumber),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        });
        
        var toggleState = new ToggleState { State = state };
        var capability = channel switch
        {
            Channel.One => new OneToggleCapability { One = toggleState },
            Channel.Two => new TwoToggleCapability { Two = toggleState },
            Channel.Three  => new ThreeToggleCapability { Three = toggleState },
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };
        
        var update = new UpdateDeviceState(capability);
        try
        {
            await MakeRequest<EmptyData>($"devices/{serialNumber}", HttpMethod.Put, update);
        }
        catch (RequestException ex) when(ex.Error is 110006)
        {
            return false;
        }
        
        return true;
    }

    public async Task<bool> SetPercentage(string serialNumber, int percent)
    {
        _ = GetDeviceForUpdate<PercentageCapability>(serialNumber);

        var update = new UpdateDeviceState(new PercentageCapability { Value = percent });
        try
        {
            await MakeRequest<EmptyData>($"devices/{serialNumber}", HttpMethod.Put, update);
        }
        catch (RequestException ex) when(ex.Error is 110006)
        {
            return false;
        }
        
        return true;
    }

    public string EnsureAccessToken()
    {
        if (AccessToken is null)
            throw new UnauthorizedAccessException("Access token was not found.");
        
        return AccessToken!;
    }

    public void Dispose()
    {
        StopEventStream().Wait();
    }

    async Task<T> ILinkControl.MakeRequest<T>(string path, HttpMethod? method, object? content)
        => await MakeRequest<T>(path, method, content);

    private async Task EnsureInitialised()
    {
        if (initialized)
            return;
        await GetDevices();
        initialized = true;
    }

    private async Task<ISubDevice> GetDeviceForUpdate<T>(string serialNumber)
        where T : Capability
    {
        return await GetDeviceForUpdate(serialNumber, (device) =>
        {
            if (!device.HasCapability<T>(Permission.Update))
                throw new NotSupportedCapabilityException<T>();
        });
    }
    
    private async Task<ISubDevice> GetDeviceForUpdate(string serialNumber, Action<ISubDevice> checkCapability)
    {
        await EnsureInitialised();
        
        if (!deviceCache.TryGetDevice(serialNumber, out var device))
            throw new UnknownDeviceException(serialNumber);

        checkCapability(device!);
        
        return device!;
    }
    
    private async Task StartEventStream()
    {
        EnsureAccessToken();

        await _lock.WaitAsync();
        try
        {
            if (eventStream == null)
            {
                await GetDevices();
                eventStream = new EventStream(this, httpClientFactory, deviceCache, loggerFactory.CreateLogger<EventStream>());
                eventStream.StateUpdated += OnStreamStateUpdate;
            }

            await eventStream.Start();
        }
        finally
        {
            _lock.Release();
        }
    }

    private void OnStreamStateUpdate(ILinkEvent<SubDeviceState> @event) => deviceStateUpdatedEvent?.Invoke(this, @event);

    private async Task StopEventStream()
    {
        await _lock.WaitAsync();
        try
        {
            if (eventStream != null)
            {
                eventStream.StateUpdated -= OnStreamStateUpdate;
                await eventStream.Stop();
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<T> MakeRequest<T>(string path, HttpMethod? method = null, object? content = null, bool negotiateApiVersion = true)
    {
        if (negotiateApiVersion)
            await NegotiateApiVersion();
        
        async Task<T> TryMakeRequest(int withPort)
        {
            UriBuilder uri = new($"http://{IpAddress}")
            {
                Path = $"{BasePath}{path}",
                Port = withPort
            };

            var httpClient = this.httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(method ?? HttpMethod.Get, uri.ToString());
            
            var contentAsJson = content is null ? string.Empty : JsonConvert.SerializeObject(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Converters = [ new PermissionConverter(ApiVersion), new StringEnumConverter() ] });
            request.Content = new StringContent(contentAsJson)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };
            if (!string.IsNullOrEmpty(AccessToken))
                request.Headers.Add("Authorization", $"Bearer {AccessToken}");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            portLocked = true;
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<OpenApiResponse<T>>(responseContent, new PermissionConverter(ApiVersion));
            return GetResponseData<T>(responseObject!);
        }

        try
        {
            return await TryMakeRequest(port);
        }
        catch (HttpRequestException ex) when (this.port == 80 && !portLocked)
        {
            logger.LogDebug("Failed to make request. Trying port 8081. reason: {Reason}", ex.Message);
            this.port = 8081;
        }
        
        return await TryMakeRequest(port);
    }

    private async Task NegotiateApiVersion()
    {
        if (apiLocked)
            return;

        try
        {
            await this.MakeRequest<string>("bridge/access_token", negotiateApiVersion: false);
            apiLocked = true;
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("Bad Request") && !apiLocked)
        {
        }
        catch (RequestException ex) when (ex.Error == (int)HttpStatusCode.BadRequest && !apiLocked)
        {
        }
        
        logger.LogDebug("Failed to make request to Api Version {ApiVersion}.", apiVersion);
        this.apiVersion = ApiVersion.v1;
        if (!portLocked)
            port = 80;
    }

    private T GetResponseData<T>(OpenApiResponse<T> response)
    {
        int error = response.Error;
        if (error != 0)
        {
            string? message = error switch
            {
                110000 => "The sub-device/group corresponding to the id does not exist",
                110001 => "The gateway is in the state of discovering zigbee devices",
                110002 => "Devices in a group do not have a common capability",
                110003 => "Incorrect number of devices",
                110004 => "Incorrect number of groups",
                110005 => "Device Offline",
                110006 => "Failed to update device status",
                110007 => "Failed to update group status",
                110008 => "The maximum number of groups has been reached. Create up to 50 groups",
                110009 => "The IP address of the camera device is incorrect",
                110010 => "Camera Device Access Authorization Error",
                110011 => "Camera device stream address error",
                110012 => "Camera device video encoding is not supported",
                110013 => "Device already exists",
                110014 => "Camera does not support offline operation",
                110015 => "The account password is inconsistent with the account password in the RTSP stream address",
                110016 => "The gateway is in the state of discovering onvif cameras",
                110017 => "Exceeded the maximum number of cameras added",
                110018 => "The path of the ESP camera is wrong",
                110019 => "Failed to access the service address of the third-party device",
                _ => response.Message
            };
            
            throw new RequestException(error, message ?? "Unknown error");
        }

        return response.Data!;
    }

    public class OpenApiResponse<T>
    {
        [JsonProperty("error")]
        public int Error { get; set; }

        [JsonProperty("data")]
        public T? Data { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }
    }

    public record EmptyData;

    public class AccessTokenData
    {
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;   
        
        
        [JsonProperty("app_name")]
        public string? AppName { get; set; }
    }

    public class Devices
    {
        [JsonProperty("device_list")]
        public List<SubDevice> List { get; set; } = new();
    }

    public class UpdateDeviceState
    {
        public UpdateDeviceState(params Capability[] capabilities)
            : this(capabilities.ToList())
        {
        }

        public UpdateDeviceState(IEnumerable<Capability> capabilities)
        {
            State = capabilities.ToDictionary(x => x.GetCapabilityName());
        }
        
        [JsonProperty("name")]

        public string? Name { get; } = null;
        
        [JsonProperty("tags")]
        public Dictionary<string, object>? Tags { get; } = null;
        
        [JsonProperty("state")]
        public Dictionary<string, Capability>? State { get; } = null;
        
        [JsonProperty("configuration")]
        public object? Configuration { get; } = null;
    }
}