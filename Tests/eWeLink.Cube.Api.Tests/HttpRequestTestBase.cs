using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace eWeLink.Cube.Api.Tests;

public class HttpRequestTestBase
{
    private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
    private readonly HttpClient httpClient;
    private readonly Mock<HttpMessageHandler> handlerMock;

    protected HttpRequestTestBase()
    {
        // Create a mock message handler
        handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        httpClient = new HttpClient(handlerMock.Object);

        // Create a mock IHttpClientFactory
        mockHttpClientFactory = new Mock<IHttpClientFactory>();

        // Setup the factory to return the mocked HttpClient
        mockHttpClientFactory
            .Setup(_ => _.CreateClient(It.IsAny<string>()))
            .Returns(Client);
    }

    protected Mock<IHttpClientFactory> HttpClientFactory => mockHttpClientFactory;

    public HttpClient Client => httpClient;
    
    public void ConfigureHttpJsonResponse(object data, Expression<Func<HttpRequestMessage, bool>>? requestExpression = null)
        => ConfigureHttpJsonResponse(JsonConvert.SerializeObject(data), requestExpression);

    public void ConfigureHttpJsonResponse(string json, Expression<Func<HttpRequestMessage, bool>>? requestExpression = null)
        => ConfigureHttpResponse(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json, new MediaTypeHeaderValue("application/json"))
        }, requestExpression);
    
    public void ConfigureHttpResponse(HttpResponseMessage httpResponseMessage, Expression<Func<HttpRequestMessage, bool>>? requestExpression = null)
    {
        handlerMock
            .Protected()
            // Setup the method to be mocked
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                requestExpression is null ?  ItExpr.IsAny<HttpRequestMessage>() : ItExpr.Is<HttpRequestMessage>(requestExpression),
                ItExpr.IsAny<CancellationToken>())
            // Prepare the expected response of the mocked HttpClient
            .ReturnsAsync(httpResponseMessage)
            .Verifiable();
    }

    public void VerifyHttpRequest()
    {
        handlerMock.Verify();
    }
}