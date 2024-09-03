using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EWeLink.Cube.Api.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api;

public class Link : ILink
{
    private const string BasePath = "open-api/v1/rest/";
    private readonly ILogger<Link> _logger;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<Link> logger;
    private int port = 80;

    public Link(IPAddress ipAddress, string? accessToken, IHttpClientFactory httpClientFactory, ILogger<Link> logger)
    {
        this.IpAddress = ipAddress;
        this.AccessToken = accessToken;
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;
    }

    public IPAddress IpAddress { get; }

    public string? AccessToken { get; private set; }
    
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

    public async Task<IReadOnlyList<ISubDevice>> GetDevices()
    {
        if (AccessToken is null)
            throw new UnauthorizedAccessException("Access token was not found.");

        var deviceList = (await MakeRequest<Devices>("devices")).List;
        return deviceList;
    }

    private async Task<T> MakeRequest<T>(string path, HttpMethod? method = null,  HttpContent? content = null)
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
            request.Content = content ?? new StringContent(string.Empty)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };
            if (!string.IsNullOrEmpty(AccessToken))
                request.Headers.Add("Authorization", $"Bearer {AccessToken}");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<OpenApiResponse<T>>(responseContent);
            return GetResponseData<T>(responseObject!);
        }

        try
        {
            return await TryMakeRequest(port);
        }
        catch (HttpRequestException ex) when (this.port == 80)
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

    public class AccessTokenData
    {
        [JsonProperty("token")]
        public string Token { get; set; }   
    }
    
    public class Devices
    {
        [JsonProperty("device_list")]
        public List<SubDevice> List { get; set; }
    }
}