using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FoodStorageApi.Infrastructure.IntegrationTests.Services;

public class BaseWebServiceClientIntegrationTests : IDisposable
{
  private readonly HttpClient _httpClient;
  private readonly IBaseWebServiceClient _webServiceClient;
  private readonly ILogger<BaseWebServiceClient> _logger;

  public BaseWebServiceClientIntegrationTests()
  {
    _httpClient = new HttpClient();
    _logger = new LoggerFactory().CreateLogger<BaseWebServiceClient>();
    _webServiceClient = new BaseWebServiceClient(_httpClient, _logger);
  }

  [Fact]
  public async Task GetAsync_WithRealHttpClient_ReturnsExpectedResponse()
  {
    // Arrange - using httpbin.org for testing
    const string endpoint = "https://httpbin.org/json";

    // Act
    var result = await _webServiceClient.GetAsync(endpoint);

    // Assert
    Assert.NotNull(result);
    Assert.Contains("slideshow", result); // httpbin.org/json returns a JSON with slideshow data
  }

  [Fact]
  public async Task PostAsync_WithRealHttpClient_ReturnsExpectedResponse()
  {
    // Arrange - using httpbin.org for testing
    const string endpoint = "https://httpbin.org/post";
    const string content = "test content";

    // Act
    var result = await _webServiceClient.PostAsync(endpoint, content);

    // Assert
    Assert.NotNull(result);
    Assert.Contains("\"data\": \"test content\"", result); // httpbin echoes back the data
  }

  [Fact]
  public async Task PutAsync_WithRealHttpClient_ReturnsExpectedResponse()
  {
    // Arrange - using httpbin.org for testing
    const string endpoint = "https://httpbin.org/put";
    const string content = "test content";

    // Act
    var result = await _webServiceClient.PutAsync(endpoint, content);

    // Assert
    Assert.NotNull(result);
    Assert.Contains("\"data\": \"test content\"", result); // httpbin echoes back the data
  }

  [Fact]
  public async Task DeleteAsync_WithRealHttpClient_ReturnsExpectedResponse()
  {
    // Arrange - using httpbin.org for testing
    const string endpoint = "https://httpbin.org/delete";

    // Act
    var result = await _webServiceClient.DeleteAsync(endpoint);

    // Assert
    Assert.NotNull(result);
    Assert.Contains("\"url\": \"https://httpbin.org/delete\"", result);
  }

  [Fact]
  public async Task GetAsync_WithNonExistentEndpoint_ThrowsHttpRequestException()
  {
    // Act & Assert
    await Assert.ThrowsAsync<HttpRequestException>(
        () => _webServiceClient.GetAsync("https://httpbin.org/status/404"));
  }

  [Fact]
  public async Task PostAsync_WithJsonContent_SendsCorrectContentType()
  {
    // Arrange - using httpbin.org which echoes headers
    const string endpoint = "https://httpbin.org/post";
    const string jsonContent = "{\"name\":\"test\",\"value\":123}";

    // Act
    var result = await _webServiceClient.PostAsync(endpoint, jsonContent);

    // Assert
    Assert.NotNull(result);
    Assert.Contains("application/json", result);
    Assert.Contains("\"data\": \"{\\\"name\\\":\\\"test\\\",\\\"value\\\":123}\"", result);
  }

  [Fact]
  public async Task GetAsync_WithTimeout_ThrowsTaskCanceledException()
  {
    // Arrange - using httpbin delay endpoint with short timeout
    using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

    // Act & Assert
    await Assert.ThrowsAnyAsync<OperationCanceledException>(
        () => _webServiceClient.GetAsync("https://httpbin.org/delay/5", cts.Token));
  }

  public void Dispose()
  {
    _httpClient?.Dispose();
  }
}