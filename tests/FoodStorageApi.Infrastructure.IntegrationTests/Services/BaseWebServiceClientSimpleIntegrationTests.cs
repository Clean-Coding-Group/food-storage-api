using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FoodStorageApi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FoodStorageApi.Infrastructure.IntegrationTests.Services;

public class BaseWebServiceClientSimpleIntegrationTests : IDisposable
{
  private readonly HttpClient _httpClient;
  private readonly BaseWebServiceClient _webServiceClient;
  private readonly ILogger<BaseWebServiceClient> _logger;

  public BaseWebServiceClientSimpleIntegrationTests()
  {
    // Use a simple HttpClient for testing
    _httpClient = new HttpClient();

    // Create a logger
    var services = new ServiceCollection()
        .AddLogging(builder => builder.AddConsole())
        .BuildServiceProvider();

    _logger = services.GetRequiredService<ILogger<BaseWebServiceClient>>();
    _webServiceClient = new BaseWebServiceClient(_httpClient, _logger);
  }

  public void Dispose()
  {
    _httpClient?.Dispose();
    _webServiceClient?.Dispose();
  }

  [Fact]
  public async Task BaseWebServiceClient_CanMakeRealHttpRequests_ToHttpBin()
  {
    // Arrange
    _httpClient.BaseAddress = new Uri("https://httpbin.org");

    // Act & Assert - Test GET
    var getResponse = await _webServiceClient.GetAsync("/json", CancellationToken.None);
    Assert.NotNull(getResponse);
    Assert.NotEmpty(getResponse);

    // Act & Assert - Test POST
    var postContent = """{"test": "data"}""";
    var postResponse = await _webServiceClient.PostAsync("/post", postContent, CancellationToken.None);
    Assert.NotNull(postResponse);
    Assert.Contains("test", postResponse);
    Assert.Contains("data", postResponse);
  }

  [Fact]
  public async Task BaseWebServiceClient_HandlesHttpErrors_Appropriately()
  {
    // Arrange
    _httpClient.BaseAddress = new Uri("https://httpbin.org");

    // Act & Assert
    await Assert.ThrowsAsync<HttpRequestException>(
        () => _webServiceClient.GetAsync("/status/404", CancellationToken.None));
  }

  [Fact]
  public async Task BaseWebServiceClient_WithTimeout_HandlesCancellation()
  {
    // Arrange
    _httpClient.BaseAddress = new Uri("https://httpbin.org");
    using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

    // Act & Assert
    await Assert.ThrowsAnyAsync<OperationCanceledException>(
        () => _webServiceClient.GetAsync("/delay/5", cts.Token));
  }

  [Fact]
  public async Task BaseWebServiceClient_SendsCorrectContentType_ForJsonRequests()
  {
    // Arrange
    _httpClient.BaseAddress = new Uri("https://httpbin.org");
    var jsonContent = """{"name": "test", "value": 123}""";

    // Act
    var response = await _webServiceClient.PostAsync("/post", jsonContent, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.Contains("application/json", response);
    Assert.Contains("name", response);
    Assert.Contains("test", response);
  }

  [Fact]
  public void BaseWebServiceClient_ImplementsIDisposable()
  {
    // Arrange & Act
    using var client = new BaseWebServiceClient(_httpClient, _logger);

    // Assert
    Assert.NotNull(client);
  }

  [Fact]
  public async Task BaseWebServiceClient_HandlesPutRequests()
  {
    // Arrange
    _httpClient.BaseAddress = new Uri("https://httpbin.org");
    var content = """{"updated": true}""";

    // Act
    var response = await _webServiceClient.PutAsync("/put", content, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.Contains("updated", response);
    Assert.Contains("true", response);
  }

  [Fact]
  public async Task BaseWebServiceClient_HandlesDeleteRequests()
  {
    // Arrange
    _httpClient.BaseAddress = new Uri("https://httpbin.org");

    // Act
    var response = await _webServiceClient.DeleteAsync("/delete", CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.NotEmpty(response);
  }
}