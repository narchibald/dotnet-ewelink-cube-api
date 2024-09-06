using System.Net;
using EWeLink.Cube.Api.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Tests.Hardware;

public class PlaySoundTests : HttpRequestTestBase
{
    private readonly IPAddress ipAddress = IPAddress.Parse("192.168.67.24");
    private readonly string accessToken = "bf6254a6-9062-4883-bbc3-4bda6af01f17";
    
    [Fact]
    public async Task PlaySound()
    {
        // Arrange
        var expectUri = new Uri($"http://{ipAddress}/open-api/v1/rest/hardware/speaker");
        var jsonData = new
        {
            error = 0,
            data = new { },
            message = "success"
        };

        string? sentJson = null;
        ConfigureHttpJsonResponse(jsonData, message => message.Method == HttpMethod.Post && message.RequestUri == expectUri && message.Content!.Headers.ContentType!.MediaType == "application/json",
            (message, token) => sentJson = message.Content!.ReadAsStringAsync().Result);
        
        // Act
        var link = new Link(ipAddress, accessToken, HttpClientFactory.Object, new DeviceCache(), Mock.Of<ILoggerFactory>());

        var result = await link.Hardware.PlaySound(new PlaySound(SoundResource.Doorbell2, 64, 3));
        
        // Assert
        VerifyHttpRequest();
        Assert.True(result);
        var json = JsonConvert.DeserializeObject<dynamic>(sentJson);
        string type = json.type;
        string name = json.sound.name;
        int volume = json.sound.volume;
        int countdown = json.sound.countdown;
        Assert.Equal("play_sound", type);
        Assert.Equal("doorbell2", name);
        Assert.Equal(64, volume);
        Assert.Equal(3, countdown);
    }
    
    [Fact]
    public async Task PlayBeep()
    {
        // Arrange
        var expectUri = new Uri($"http://{ipAddress}/open-api/v1/rest/hardware/speaker");
        var jsonData = new
        {
            error = 0,
            data = new { },
            message = "success"
        };

        string? sentJson = null;
        ConfigureHttpJsonResponse(jsonData, message => message.Method == HttpMethod.Post && message.RequestUri == expectUri && message.Content!.Headers.ContentType!.MediaType == "application/json",
            (message, token) => sentJson = message.Content!.ReadAsStringAsync().Result);
        
        // Act
        var link = new Link(ipAddress, accessToken, HttpClientFactory.Object, new DeviceCache(), Mock.Of<ILoggerFactory>());

        var result = await link.Hardware.PlaySound(new PlayBeep(BeepResource.SystemArmed, 64));
        
        // Assert
        VerifyHttpRequest();
        Assert.True(result);
        var json = JsonConvert.DeserializeObject<dynamic>(sentJson);
        string type = json.type;
        string name = json.beep.name;
        int volume = json.beep.volume;
        Assert.Equal("play_beep", type);
        Assert.Equal("systemArmed", name);
        Assert.Equal(64, volume);
    }
}