using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;

namespace FoodStorageApi.Infrastructure.IntegrationTests.Services;

public class BaseWebServiceClientIntegrationTests : IClassFixture<WebApplicationFactory<TestStartup>>
{
  private readonly WebApplicationFactory<TestStartup> _factory;
  private readonly HttpClient _httpClient;
  private readonly IBaseWebServiceClient _webServiceClient;

  public BaseWebServiceClientIntegrationTests(WebApplicationFactory<TestStartup> factory)
  {
    _factory = factory;

    // Use the factory directly for the test endpoints
    _httpClient = _factory.CreateClient();

    // Get logger from the factory's service provider
    using var scope = _factory.Services.CreateScope();
    var logger = scope.ServiceProvider.GetService<ILogger<BaseWebServiceClient>>()
                 ?? new LoggerFactory().CreateLogger<BaseWebServiceClient>();
    _webServiceClient = new BaseWebServiceClient(_httpClient, logger);
  }

  [Fact]
  public async Task GetAsync_WithRealHttpClient_ReturnsExpectedResponse()
  {
    // Arrange
    const string expectedResponse = "Hello from GET endpoint";

    // Act
    var result = await _webServiceClient.GetAsync("/test-get");

    // Assert
    Assert.Equal(expectedResponse, result);
  }

  [Fact]
  public async Task PostAsync_WithRealHttpClient_ReturnsExpectedResponse()
  {
    // Arrange
    const string content = "test content";
    const string expectedResponse = "Hello from POST endpoint";

    // Act
    var result = await _webServiceClient.PostAsync("/test-post", content);

    // Assert
    Assert.Equal(expectedResponse, result);
  }

  [Fact]
  public async Task PutAsync_WithRealHttpClient_ReturnsExpectedResponse()
  {
    // Arrange
    const string content = "test content";
    const string expectedResponse = "Hello from PUT endpoint";

    // Act
    var result = await _webServiceClient.PutAsync("/test-put", content);

    // Assert
    Assert.Equal(expectedResponse, result);
  }

  [Fact]
  public async Task DeleteAsync_WithRealHttpClient_ReturnsExpectedResponse()
  {
    // Arrange
    const string expectedResponse = "Hello from DELETE endpoint";

    // Act
    var result = await _webServiceClient.DeleteAsync("/test-delete");

    // Assert
    Assert.Equal(expectedResponse, result);
  }

  [Fact]
  public async Task GetAsync_WithNonExistentEndpoint_ThrowsHttpRequestException()
  {
    // Act & Assert
    await Assert.ThrowsAsync<HttpRequestException>(
        () => _webServiceClient.GetAsync("/non-existent"));
  }

  [Fact]
  public async Task PostAsync_WithJsonContent_SendsCorrectContentType()
  {
    // Arrange
    const string jsonContent = "{\"name\":\"test\",\"value\":123}";

    // Act
    var result = await _webServiceClient.PostAsync("/test-post-json", jsonContent);

    // Assert
    Assert.Contains("application/json", result);
  }

  [Fact]
  public async Task GetAsync_WithTimeout_ThrowsTaskCanceledException()
  {
    // Arrange
    using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

    // Act & Assert
    await Assert.ThrowsAnyAsync<OperationCanceledException>(
        () => _webServiceClient.GetAsync("/test-delay", cts.Token));
  }
}

// Test startup class for creating mock endpoints
public class TestStartup
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddRouting();
    services.AddLogging();
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapGet("/test-get", async context =>
          {
            await context.Response.WriteAsync("Hello from GET endpoint");
          });

      endpoints.MapPost("/test-post", async context =>
          {
            await context.Response.WriteAsync("Hello from POST endpoint");
          });

      endpoints.MapPut("/test-put", async context =>
          {
            await context.Response.WriteAsync("Hello from PUT endpoint");
          });

      endpoints.MapDelete("/test-delete", async context =>
          {
            await context.Response.WriteAsync("Hello from DELETE endpoint");
          });

      endpoints.MapPost("/test-post-json", async context =>
          {
            var contentType = context.Request.ContentType;
            await context.Response.WriteAsync($"Content-Type: {contentType}");
          });

      endpoints.MapGet("/test-delay", async context =>
          {
            await Task.Delay(2000); // Simulate slow endpoint
            await context.Response.WriteAsync("Delayed response");
          });
    });
  }
}