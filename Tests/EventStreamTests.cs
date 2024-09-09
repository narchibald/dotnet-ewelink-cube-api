using System.Net;
using System.Text;
using EWeLink.Cube.Api.Models;
using EWeLink.Cube.Api.Models.Devices;
using EWeLink.Cube.Api.Models.States;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Nito.AsyncEx;

namespace EWeLink.Cube.Api.Tests;

public class EventStreamTests : HttpRequestTestBase
{
    private readonly IPAddress ipAddress = IPAddress.Parse("192.168.67.24");
    private readonly string accessToken = "bf6254a6-9062-4883-bbc3-4bda6af01f17";

    
    public async Task UpdateStateEvent()
    {
        // Arrange
        var expectUri = new Uri($"http://{ipAddress}//open-api/v1/sse/bridge");
        AsyncManualResetEvent manualResetEvent = new(false);
        using var memoryStream = new MemoryStream();
        var writer = new StreamWriter(memoryStream, Encoding.UTF8);
        ConfigureHttpStreamResponse(memoryStream, message => message.Method == HttpMethod.Put && message.RequestUri == expectUri && message.Content!.Headers.ContentType!.MediaType == "application/json");

        var cache = new DeviceCache();
        cache.UpdateCache(new ZbMicro()
        {
            SerialNumber = "982376"
        });
        
       
        ILinkEvent<SubDeviceState> @firedEvent = null;
        void UpdateEventHandler(ILink link, ILinkEvent<SubDeviceState> @event)
        {
            @firedEvent = @event;
            manualResetEvent.Set();
        }

        var json = JsonConvert.SerializeObject(new
        {
            endpoint = new
            {
                serial_number = "982376",
            },
            payload = new
            {
                power = new
                {
                    powerState = "on"
                }
            }
        });
        
        await writer.WriteLineAsync("event: device#v1#updateDeviceState");
        await writer.WriteLineAsync("data: " + json.Take(20));
        await writer.WriteLineAsync("data: " + json.Skip(20));
        await writer.WriteLineAsync("");
        await writer.FlushAsync();
        memoryStream.Seek(0, SeekOrigin.Begin);
        
        var link = new Link(ipAddress, accessToken, HttpClientFactory.Object, cache, Mock.Of<ILoggerFactory>());
        link.DeviceStateUpdated += UpdateEventHandler;

        
        // Act

        // await Task.Delay(10000);
        CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(20));
        await manualResetEvent.WaitAsync(cancellationTokenSource.Token);
        
        
        // Assert
        Assert.NotNull(firedEvent);
    }
}