using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using EWeLink.Cube.Api.Models.Devices;
using EWeLink.Cube.Api.Models.States;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api;

public class Link : ILink, ILinkControl
{
    private const string BasePath = "open-api/v1/rest/";
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IDeviceCache deviceCache;
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<Link> logger;
    private EventStream? eventStream;
    private int port = 80;
    private bool portLocked = false; 
    private Action<ILink, ILinkEvent<SubDeviceState>>? deviceStateUpdatedEvent;

    public Link(IPAddress ipAddress, string? accessToken, IHttpClientFactory httpClientFactory, IDeviceCache deviceCache, ILoggerFactory loggerFactory)
    {
        this.IpAddress = ipAddress;
        this.AccessToken = accessToken;
        this.httpClientFactory = httpClientFactory;
        this.deviceCache = deviceCache;
        this.loggerFactory = loggerFactory;
        this.logger = loggerFactory.CreateLogger<Link>();
        Gateway = new Gateway(this);
        Screen = new Screen(this);
        Hardware = new Hardware(this);
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

    public int Port => port;
    
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

    public async Task<IReadOnlyList<ISubDevice>> GetDevices()
    {
        EnsureAccessToken();

        var deviceList = (await MakeRequest<Devices>("devices")).List;
        deviceCache.UpdateCache(deviceList);
        return deviceList;
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

    private async Task<T> MakeRequest<T>(string path, HttpMethod? method = null, object? content = null)
    {
        async Task<T> TryMakeRequest(int withPort)
        {
            UriBuilder uri = new($"http://{IpAddress}")
            {
                Path = $"{BasePath}{path}",
                Port = withPort
            };

            var httpClient = this.httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(method ?? HttpMethod.Get, uri.ToString());
            
            var contentAsJson = content is null ? string.Empty : JsonConvert.SerializeObject(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
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
            var responseObject = JsonConvert.DeserializeObject<OpenApiResponse<T>>(responseContent);
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

    private T GetResponseData<T>(OpenApiResponse<T> response)
    {
        int error = response.Error;
        if (error != 0)
        {
            string? message = response.Message;
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
}