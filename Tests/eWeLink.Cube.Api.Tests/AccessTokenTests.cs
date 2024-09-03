using System.Net;
using System.Net.Http.Headers;
using EWeLink.Cube.Api;
using Microsoft.Extensions.Logging;
using Moq;

namespace eWeLink.Cube.Api.Tests;

public class AccessTokenTests : HttpRequestTestBase
{
    private readonly IPAddress ipAddress = IPAddress.Parse("192.168.67.24");
    
    [Fact]
    public async Task ValidRequest_ValidToken()
    {
        // Arrange
        var expectUri = new Uri($"http://{ipAddress}/open-api/v1/rest/bridge/access_token");
        var expectedToken = "376310da-7adf-4521-b18c-5e0752cfff8d";
        var jsonData = new {
            error = 0,
            data = new { token = expectedToken },
            message = "success",
        };
        ConfigureHttpJsonResponse(jsonData, message => message.Method == HttpMethod.Get && message.RequestUri == expectUri && message.Content.Headers.ContentType.MediaType == "application/json");
        
        // Act
        var link = new Link(ipAddress, null, HttpClientFactory.Object, Mock.Of<ILogger<Link>>());

        var accessToken = await link.GetAccessToken();

        // Assert
        VerifyHttpRequest();
        Assert.Equal(expectedToken, accessToken);
        Assert.Equal(expectedToken, link.AccessToken);
    }
}