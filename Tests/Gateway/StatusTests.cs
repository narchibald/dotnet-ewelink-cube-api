using System.Net;
using Microsoft.Extensions.Logging;
using Moq;

namespace EWeLink.Cube.Api.Tests.Gateway;

public class StatusTests : HttpRequestTestBase
{
    private readonly IPAddress ipAddress = IPAddress.Parse("192.168.67.24");
    private readonly string accessToken = "bf6254a6-9062-4883-bbc3-4bda6af01f17";
    
    [Fact]
    public async Task GetStatus()
    {
        // Arrange
        var expectUri = new Uri($"http://{ipAddress}/open-api/v1/rest/bridge/runtime");
        var expectedPowerUpTime = "2024-03-03T12:48:09.989Z";
        var jsonData = new
        {
            error = 0,
            data = new
            {
                ram_used = 40,
                cpu_used = 30,
                power_up_time = DateTime.Parse(expectedPowerUpTime)
            },
            message = "success"
        };
        ConfigureHttpJsonResponse(jsonData, message => message.Method == HttpMethod.Get && message.RequestUri == expectUri && message.Content.Headers.ContentType.MediaType == "application/json");
        
        // Act
        var link = new Link(ipAddress, accessToken, HttpClientFactory.Object, Mock.Of<ILogger<Link>>());

        var status = await link.Gateway.GetStatus();
        
        // Assert
        VerifyHttpRequest();
        Assert.NotNull(status);
        Assert.Equal(40, status.RamUsed);
        Assert.Equal(30, status.CpuUsed);
        Assert.Equal(DateTimeOffset.Parse(expectedPowerUpTime), status.PowerUpTime);
    }
}