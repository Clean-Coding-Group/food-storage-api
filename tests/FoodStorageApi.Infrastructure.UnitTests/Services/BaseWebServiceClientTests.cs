using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FoodStorageApi.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace FoodStorageApi.Infrastructure.UnitTests.Services;

public class BaseWebServiceClientTests : IDisposable
{
  private readonly Mock<ILogger<BaseWebServiceClient>> _mockLogger;
  private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
  private readonly HttpClient _httpClient;
  private readonly BaseWebServiceClient _webServiceClient;

  public BaseWebServiceClientTests()
  {
    _mockLogger = new Mock<ILogger<BaseWebServiceClient>>();
    _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
    {
      BaseAddress = new Uri("https://api.test.com")
    };
    _webServiceClient = new BaseWebServiceClient(_httpClient, _mockLogger.Object);
  }

  public void Dispose()
  {
    _httpClient?.Dispose();
    _webServiceClient?.Dispose();
  }

  [Fact]
  public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
  {
    // Arrange & Act & Assert
    Assert.Throws<ArgumentNullException>(() =>
        new BaseWebServiceClient(null!, _mockLogger.Object));
  }

  [Fact]
  public void Constructor_WithNullLogger_ThrowsArgumentNullException()
  {
    // Arrange & Act & Assert
    Assert.Throws<ArgumentNullException>(() =>
        new BaseWebServiceClient(_httpClient, null!));
  }

  [Fact]
  public async Task GetAsync_WithValidEndpoint_ReturnsResponse()
  {
    // Arrange
    const string endpoint = "api/test";
    const string expectedResponse = "test response";

    _mockHttpMessageHandler.Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(new HttpResponseMessage
        {
          StatusCode = HttpStatusCode.OK,
          Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
        });

    // Act
    var result = await _webServiceClient.GetAsync(endpoint);

    // Assert
    Assert.Equal(expectedResponse, result);
  }

  [Fact]
  public async Task GetAsync_WithNullEndpoint_ThrowsArgumentNullException()
  {
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentNullException>(() =>
        _webServiceClient.GetAsync(null!));
  }

  [Fact]
  public async Task GetAsync_WithHttpError_ThrowsHttpRequestException()
  {
    // Arrange
    const string endpoint = "api/error";

    _mockHttpMessageHandler.Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(new HttpResponseMessage
        {
          StatusCode = HttpStatusCode.NotFound
        });

    // Act & Assert
    await Assert.ThrowsAsync<HttpRequestException>(() =>
        _webServiceClient.GetAsync(endpoint));
  }

  [Fact]
  public async Task PostAsync_WithValidData_ReturnsResponse()
  {
    // Arrange
    const string endpoint = "api/test";
    const string content = "test content";
    const string expectedResponse = "created response";

    _mockHttpMessageHandler.Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(new HttpResponseMessage
        {
          StatusCode = HttpStatusCode.Created,
          Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
        });

    // Act
    var result = await _webServiceClient.PostAsync(endpoint, content);

    // Assert
    Assert.Equal(expectedResponse, result);
  }

  [Fact]
  public async Task PostAsync_WithNullEndpoint_ThrowsArgumentNullException()
  {
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentNullException>(() =>
        _webServiceClient.PostAsync(null!, "content"));
  }

  [Fact]
  public async Task PostAsync_WithNullContent_ThrowsArgumentNullException()
  {
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentNullException>(() =>
        _webServiceClient.PostAsync("api/test", null!));
  }

  [Fact]
  public async Task DeleteAsync_WithValidEndpoint_ReturnsResponse()
  {
    // Arrange
    const string endpoint = "api/test";
    const string expectedResponse = "deleted response";

    _mockHttpMessageHandler.Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
            ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(new HttpResponseMessage
        {
          StatusCode = HttpStatusCode.OK,
          Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
        });

    // Act
    var result = await _webServiceClient.DeleteAsync(endpoint);

    // Assert
    Assert.Equal(expectedResponse, result);
  }

  [Fact]
  public async Task DeleteAsync_WithNullEndpoint_ThrowsArgumentNullException()
  {
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentNullException>(() =>
        _webServiceClient.DeleteAsync(null!));
  }

  [Fact]
  public async Task GetAsync_WithCancellation_ThrowsOperationCanceledException()
  {
    // Arrange
    const string endpoint = "api/test";
    using var cancellationTokenSource = new CancellationTokenSource();
    cancellationTokenSource.Cancel();

    _mockHttpMessageHandler.Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
        .ThrowsAsync(new TaskCanceledException());

    // Act & Assert
    await Assert.ThrowsAsync<TaskCanceledException>(() =>
        _webServiceClient.GetAsync(endpoint, cancellationTokenSource.Token));
  }

  [Fact]
  public async Task PutAsync_WithValidData_ReturnsResponse()
  {
    // Arrange
    const string endpoint = "api/test";
    const string content = "test content";
    const string expectedResponse = "updated response";

    _mockHttpMessageHandler.Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put),
            ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(new HttpResponseMessage
        {
          StatusCode = HttpStatusCode.OK,
          Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
        });

    // Act
    var result = await _webServiceClient.PutAsync(endpoint, content);

    // Assert
    Assert.Equal(expectedResponse, result);
  }
}