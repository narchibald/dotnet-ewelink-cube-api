using System.Net;
using EWeLink.Cube.Api.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Tests.Screen;

public class DisplayTests : HttpRequestTestBase
{
    private readonly IPAddress ipAddress = IPAddress.Parse("192.168.67.24");
    private readonly string accessToken = "bf6254a6-9062-4883-bbc3-4bda6af01f17";
    
    [Fact]
    public async Task SetDisplayAutoDisabled()
    {
        // Arrange
        var expectUri = new Uri($"http://{ipAddress}/open-api/v1/rest/screen/display");
        var jsonData = new
        {
            error = 0,
            data = new { },
            message = "success"
        };

        string? sentJson = null;
        ConfigureHttpJsonResponse(jsonData, message => message.Method == HttpMethod.Put && message.RequestUri == expectUri && message.Content!.Headers.ContentType!.MediaType == "application/json",
            (message, token) => sentJson = message.Content!.ReadAsStringAsync().Result);
        
        // Act
        var link = new Link(ipAddress, accessToken, HttpClientFactory.Object, new DeviceCache(), Mock.Of<ILoggerFactory>());

        var result = await link.Screen.SetDisplay(false);
        
        // Assert
        VerifyHttpRequest();
        Assert.True(result);
        Assert.NotNull(sentJson);
        var json = JsonConvert.DeserializeObject<dynamic>(sentJson);
        bool enable = json.auto_screen_off.enable;
        int? duration = json.auto_screen_off.duration;
        Assert.False(enable);
        Assert.Null(duration);
    }
    
    [Fact]
    public async Task SetDisplayAutoEnabled()
    {
        // Arrange
        var expectUri = new Uri($"http://{ipAddress}/open-api/v1/rest/screen/display");
        var jsonData = new
        {
            error = 0,
            data = new { },
            message = "success"
        };

        string? sentJson = null;
        ConfigureHttpJsonResponse(jsonData, message => message.Method == HttpMethod.Put && message.RequestUri == expectUri && message.Content!.Headers.ContentType!.MediaType == "application/json",
            (message, token) => sentJson = message.Content!.ReadAsStringAsync().Result);
        
        // Act
        var link = new Link(ipAddress, accessToken, HttpClientFactory.Object, new DeviceCache(), Mock.Of<ILoggerFactory>());

        var result = await link.Screen.SetDisplay(true, 65);
        
        // Assert
        VerifyHttpRequest();
        Assert.True(result);
        Assert.NotNull(sentJson);
        var json = JsonConvert.DeserializeObject<dynamic>(sentJson);
        bool enable = json.auto_screen_off.enable;
        int? duration = json.auto_screen_off.duration;
        Assert.True(enable);
        Assert.NotNull(duration);
        Assert.Equal(65, duration);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData(14)]
    [InlineData(1801)]
    public async Task SetDisplayAutoEnabledBadDuration(int? setDuration )
    {
        // Arrange
        var expectUri = new Uri($"http://{ipAddress}/open-api/v1/rest/screen/display");
        var jsonData = new
        {
            error = 0,
            data = new { },
            message = "success"
        };

        string? sentJson = null;
        ConfigureHttpJsonResponse(jsonData, message => message.Method == HttpMethod.Put && message.RequestUri == expectUri && message.Content!.Headers.ContentType!.MediaType == "application/json",
            (message, token) => sentJson = message.Content!.ReadAsStringAsync().Result);
        
        // Act
        var link = new Link(ipAddress, accessToken, HttpClientFactory.Object, new DeviceCache(), Mock.Of<ILoggerFactory>());

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => link.Screen.SetDisplay(true, setDuration));
        
        // Assert
        Assert.Null(sentJson);
    }
}