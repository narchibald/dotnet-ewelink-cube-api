using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Moq;

namespace EWeLink.Cube.Api.Tests;

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
        ConfigureHttpJsonResponse(jsonData, message => message.Method == HttpMethod.Get && message.RequestUri == expectUri && message.Content!.Headers.ContentType!.MediaType == "application/json");
        
        // Act
        var link = new Link(ipAddress, null, HttpClientFactory.Object, new DeviceCache(), Mock.Of<ILoggerFactory>());

        var accessToken = await link.GetAccessToken();

        // Assert
        VerifyHttpRequest();
        Assert.Equal(expectedToken, accessToken);
        Assert.Equal(expectedToken, link.AccessToken);
    }
    
    [Fact]
    public async Task PortChangeValidRequest_ValidToken()
    {
        // Arrange
        var expectUri = new Uri($"http://{ipAddress}:8081/open-api/v1/rest/bridge/access_token");
        var expectedToken = "376310da-7adf-4521-b18c-5e0752cfff8d";
        HttpResponseMessage?[] responses =
            [
                null,
                new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(new {
                    error = 401,
                    message = "Forbidden",
                }) },
                new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(new {
                    error = 0,
                    data = new { token = expectedToken },
                    message = "success",
                }) }
            ];
        ConfigureHttpResponseSequence(responses, message => message.Method == HttpMethod.Get && message.RequestUri!.Query == expectUri.Query && message.Content!.Headers.ContentType!.MediaType == "application/json");
        
        Mock<ILoggerFactory> loggerFactory = new();
        loggerFactory.Setup(factory => factory.CreateLogger(It.IsAny<string>())).Returns(Mock.Of<ILogger>());
        
        // Act
        var link = new Link(ipAddress, null, HttpClientFactory.Object, new DeviceCache(), loggerFactory.Object);

        var accessToken = await link.GetAccessToken();

        // Assert
        VerifyHttpRequest();
        Assert.Equal(8081, link.Port);
        Assert.Equal(expectedToken, accessToken);
        Assert.Equal(expectedToken, link.AccessToken);
    }
}