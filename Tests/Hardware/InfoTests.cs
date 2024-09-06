using System.Net;
using Microsoft.Extensions.Logging;
using Moq;

namespace EWeLink.Cube.Api.Tests.Hardware;

public class RestartTests : HttpRequestTestBase
{
    private readonly IPAddress ipAddress = IPAddress.Parse("192.168.67.24");
    private readonly string accessToken = "bf6254a6-9062-4883-bbc3-4bda6af01f17";
    
    [Fact]
    public async Task Reboot()
    {
        // Arrange
        var expectUri = new Uri($"http://{ipAddress}/open-api/v1/rest/hardware/reboot");
        var jsonData = new
        {
            error = 0,
            data = new { },
            message = "success"
        };
        ConfigureHttpJsonResponse(jsonData, message => message.Method == HttpMethod.Post && message.RequestUri == expectUri && message.Content.Headers.ContentType.MediaType == "application/json");
        
        // Act
        var link = new Link(ipAddress, accessToken, HttpClientFactory.Object, new DeviceCache(), Mock.Of<ILoggerFactory>());

        var result = await link.Hardware.Reboot();
        
        // Assert
        VerifyHttpRequest();
        Assert.True(result);
    }
}