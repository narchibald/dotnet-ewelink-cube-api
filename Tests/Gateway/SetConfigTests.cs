using System.Net;
using EWeLink.Cube.Api.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace EWeLink.Cube.Api.Tests.Gateway;

public class SetConfigTests : HttpRequestTestBase
{
    private readonly IPAddress ipAddress = IPAddress.Parse("192.168.67.24");
    private readonly string accessToken = "bf6254a6-9062-4883-bbc3-4bda6af01f17";
    
    [Fact]
    public async Task SetConfig()
    {
        // Arrange
        var expectUri = new Uri($"http://{ipAddress}/open-api/v1/rest/bridge/config");
        var jsonData = new
        {
            error = 0,
            data = new { },
            message = "success"
        };
        ConfigureHttpJsonResponse(jsonData, message => message.Method == HttpMethod.Put && message.RequestUri == expectUri && message.Content.Headers.ContentType.MediaType == "application/json");
        
        // Act
        var link = new Link(ipAddress, accessToken, HttpClientFactory.Object, Mock.Of<ILogger<Link>>());

        var result = await link.Gateway.SetConfig(new GatewayConfig(20));
        
        // Assert
        VerifyHttpRequest();
        Assert.True(result);
    }
}