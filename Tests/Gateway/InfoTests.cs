using System.Net;
using Microsoft.Extensions.Logging;
using Moq;

namespace EWeLink.Cube.Api.Tests.Gateway;

public class InfoTests : HttpRequestTestBase
{
    private readonly IPAddress ipAddress = IPAddress.Parse("192.168.67.24");
    private readonly string accessToken = "bf6254a6-9062-4883-bbc3-4bda6af01f17";
    
    [Fact]
    public async Task GetInfo()
    {
        // Arrange
        var expectUri = new Uri($"http://{ipAddress}/open-api/v1/rest/bridge");
        var expectedPowerUpTime = "2024-03-03T12:48:09.989Z";
        var jsonData = new
        {
            error = 0,
            data = new
            {
                ip = "192.168.31.25",
                mac = "00:0A:02:0B:03:0C",
                domain = "NSPanelPro.local",
                fw_version = "1.0.0"
            },
            message = "success"
        };
        ConfigureHttpJsonResponse(jsonData, message => message.Method == HttpMethod.Get && message.RequestUri == expectUri && message.Content.Headers.ContentType.MediaType == "application/json");
        
        // Act
        var link = new Link(ipAddress, accessToken, HttpClientFactory.Object, Mock.Of<ILogger<Link>>());

        var info = await link.Gateway.GetInfo();
        
        // Assert
        VerifyHttpRequest();
        Assert.NotNull(info);
        Assert.Equal("192.168.31.25", info.IPAddress);
        Assert.Equal("00:0A:02:0B:03:0C", info.MacAddress);
        Assert.Equal("NSPanelPro.local", info.Domain);
        Assert.Equal("1.0.0", info.FirmwareVersion);
    }
}