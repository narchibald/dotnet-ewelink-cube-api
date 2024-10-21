using System.Net;
using System.Text;
using EWeLink.Cube.Api.Models;
using EWeLink.Cube.Api.Models.Capabilities;
using EWeLink.Cube.Api.Models.Devices;
using EWeLink.Cube.Api.Models.States;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Nito.AsyncEx;

namespace EWeLink.Cube.Api.Tests;

public class EventStreamTests : HttpRequestTestBase, IAsyncLifetime
{
    private readonly IPAddress ipAddress = IPAddress.Parse("192.168.67.24");
    private readonly string accessToken = "bf6254a6-9062-4883-bbc3-4bda6af01f17";
    private readonly Uri expectUri;
    private readonly AsyncAutoResetEvent autoResetEvent = new (false);
    private MemoryStream memoryStream;
    private ILinkEvent<SubDeviceState>? firedEvent;

    public EventStreamTests()
    {
        expectUri = new Uri($"http://{ipAddress}/open-api/v1/sse/bridge?access_token={accessToken}");
    }

    [Fact]
    public async Task UpdateStateEvent()
    {
        // Arrange
        var cache = new DeviceCache();
        cache.UpdateCache(new ZbMicro()
        {
            SerialNumber = "982376",
            State = new ZbMicroState()
        });

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

        await WriteLinesToStream(
            "event: device#v1#updateDeviceState",
            "data: " + json.Substring(0, 20),
            "data: " + json.Substring(20),
            "");
        
        var link = CreateLink(cache);
        link.DeviceStateUpdated += UpdateEventHandler;
        
        // Act
        await AwaitEvent();
        
        // Assert
        VerifyHttpRequest();
        Assert.NotNull(firedEvent);
        var device = cache.GetDevice("982376") as ZbMicro;
        Assert.NotNull(device);
        Assert.NotNull(device.State.Power);
        Assert.Equal(SwitchState.On, device.State.Power.State);
    }
    
    [Fact]
    public async Task UpdateStateEvent_PartEventFirst()
    {
        // Arrange
        var cache = new DeviceCache();
        cache.UpdateCache(new ZbMicro()
        {
            SerialNumber = "982376",
            State = new ZbMicroState()
        });

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

        await WriteLinesToStream(
            "event: device#v1#updateDeviceState",
            "",
            "event: device#v1#updateDeviceState",
            "data: " + json.Substring(0, 20),
            "data: " + json.Substring(20),
            "");
        
        var link = CreateLink(cache);
        link.DeviceStateUpdated += UpdateEventHandler;
        
        // Act
        await AwaitEvent();
        
        // Assert
        VerifyHttpRequest();
        Assert.NotNull(firedEvent);
        var device = cache.GetDevice("982376") as ZbMicro;
        Assert.NotNull(device);
        Assert.NotNull(device.State.Power);
        Assert.Equal(SwitchState.On, device.State.Power.State);
    }
    
    [Fact]
    public async Task UpdateStateEvent_JunkJsonEventFirst()
    {
        // Arrange
        var cache = new DeviceCache();
        cache.UpdateCache(new ZbMicro()
        {
            SerialNumber = "982376",
            State = new ZbMicroState()
        });

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

        await WriteLinesToStream(
            "event: device#v1#updateDeviceState",
            "data: { meg: \" \"", 
            "",
            "event: device#v1#updateDeviceState",
            "data: " + json.Substring(0, 20),
            "data: " + json.Substring(20),
            "");
        
        var link = CreateLink(cache);
        link.DeviceStateUpdated += UpdateEventHandler;
        
        // Act
        await AwaitEvent();
        
        // Assert
        VerifyHttpRequest();
        Assert.NotNull(firedEvent);
        var device = cache.GetDevice("982376") as ZbMicro;
        Assert.NotNull(device);
        Assert.NotNull(device.State.Power);
        Assert.Equal(SwitchState.On, device.State.Power.State);
    }
    
    [Fact]
    public async Task UpdateStateEvent_UnknownDeviceEventFirst()
    {
        // Arrange
        var cache = new DeviceCache();
        cache.UpdateCache(new ZbMicro()
        {
            SerialNumber = "982376",
            State = new ZbMicroState()
        });
        
        var jsonUnknown = JsonConvert.SerializeObject(new
        {
            endpoint = new
            {
                serial_number = "123456",
            },
            payload = new
            {
                power = new
                {
                    powerState = "on"
                }
            }
        });

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

        await WriteLinesToStream(
            "event: device#v1#updateDeviceState",
            "data: " + jsonUnknown, 
            "",
            "event: device#v1#updateDeviceState",
            "data: " + json.Substring(0, 20),
            "data: " + json.Substring(20),
            "");
        
        var link = CreateLink(cache);
        link.DeviceStateUpdated += UpdateEventHandler;
        
        // Act
        await AwaitEvent();
        
        // Assert
        VerifyHttpRequest();
        Assert.NotNull(firedEvent);
        var device = cache.GetDevice("982376") as ZbMicro;
        Assert.NotNull(device);
        Assert.NotNull(device.State.Power);
        Assert.Equal(SwitchState.On, device.State.Power.State);
    }
    
    [Fact]
    public async Task UpdateStateEvent_CrapEventKnownDevice()
    {
        // Arrange
        var cache = new DeviceCache();
        cache.UpdateCache(new ZbMicro()
        {
            SerialNumber = "982376",
            State = new ZbMicroState()
        });

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

        await WriteLinesToStream(
            "event: dev", 
            "",
            "event: device#v1#updateDeviceState",
            "data: " + json.Substring(0, 20),
            "data: " + json.Substring(20),
            "");
        
        var link = CreateLink(cache);
        link.DeviceStateUpdated += UpdateEventHandler;
        
        // Act
        await AwaitEvent();
        
        // Assert
        VerifyHttpRequest();
        Assert.NotNull(firedEvent);
        var device = cache.GetDevice("982376") as ZbMicro;
        Assert.NotNull(device);
        Assert.NotNull(device.State.Power);
        Assert.Equal(SwitchState.On, device.State.Power.State);
    }
    
    [Theory]
    [InlineData("device", "meh")]
    [InlineData("hand", "meh")]
    [InlineData("", "")]
    [InlineData("", "", "")]
    [InlineData(null, null)]
    public async Task UnknownEvent_CrapEventKnownDevice(string? target, string? eventName, string version = "v3")
    {
        // Arrange
        var cache = new DeviceCache();
        cache.UpdateCache(new ZbMicro()
        {
            SerialNumber = "982376",
            State = new ZbMicroState()
        });

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

        await WriteLinesToStream(
            target is null && eventName is null ? "event:" : $"event: {target}#{version}#{eventName}", 
            "",
            "event: device#v1#updateDeviceState",
            "data: " + json.Substring(0, 20),
            "data: " + json.Substring(20),
            "");
        
        var link = CreateLink(cache);
        link.DeviceStateUpdated += UpdateEventHandler;
        
        // Act
        await AwaitEvent();
        
        // Assert
        VerifyHttpRequest();
        Assert.NotNull(firedEvent);
        var device = cache.GetDevice("982376") as ZbMicro;
        Assert.NotNull(device);
        Assert.NotNull(device.State.Power);
        Assert.Equal(SwitchState.On, device.State.Power.State);
    }
    
    private void UpdateEventHandler(ILink link, ILinkEvent<SubDeviceState> linkEvent)
    {
        @firedEvent = @linkEvent;
        autoResetEvent.Set();
    }
    
    private Link CreateLink(DeviceCache? cache = null)
        => new Link(ipAddress, accessToken, 80, ApiVersion.v1, HttpClientFactory.Object, cache ?? new DeviceCache(), Mock.Of<ILoggerFactory>(x => x.CreateLogger(It.IsAny<string>()) == Mock.Of<ILogger>()));

    private async Task WriteLinesToStream(params string[] lines)
    {
        using StreamWriter writer = new StreamWriter(memoryStream, leaveOpen: true); 
        foreach (var line in lines)
        {
            await writer.WriteLineAsync(line);
        }

        await writer.FlushAsync();
        writer.BaseStream.Seek(0, SeekOrigin.Begin);
    }

    private async Task WaitForFullStreamRead(Stream stream)
    {
        try
        {
            while (stream.Position < stream.Length)
                await Task.Delay(2);
        }
        catch (ObjectDisposedException){}
    }

    private async Task AwaitEvent()
    {
        await WaitForFullStreamRead(memoryStream);

        try
        {
            CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(1));
            await autoResetEvent.WaitAsync(cancellationTokenSource.Token);
        }
        catch (OperationCanceledException) {}
    }

    public Task InitializeAsync()
    {
        memoryStream = new ();
        
        ConfigureHttpJsonResponse(new { error = 0, data = new { device_list = Array.Empty<string>() }, message = "success" } , message => message.Method == HttpMethod.Get && message.RequestUri.PathAndQuery.Contains("devices") && message.Content!.Headers.ContentType!.MediaType == "application/json");
        ConfigureHttpStreamResponse(memoryStream, message => message.Method == HttpMethod.Get && message.RequestUri == expectUri);

        firedEvent = null;
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}