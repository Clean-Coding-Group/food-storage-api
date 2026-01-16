using FoodStorageApi.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;

namespace FoodStorageApi.Infrastructure.Services;

public class BaseWebServiceClient : IBaseWebServiceClient, IDisposable
{
  private readonly HttpClient _httpClient;
  private readonly ILogger<BaseWebServiceClient> _logger;

  public BaseWebServiceClient(HttpClient httpClient, ILogger<BaseWebServiceClient> logger)
  {
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public async Task<string> GetAsync(string endpoint, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrEmpty(endpoint))
      throw new ArgumentNullException(nameof(endpoint));

    try
    {
      _logger.LogInformation("Making GET request to {Endpoint}", endpoint);

      var response = await _httpClient.GetAsync(endpoint, cancellationToken);
      response.EnsureSuccessStatusCode();

      var content = await response.Content.ReadAsStringAsync(cancellationToken);

      _logger.LogInformation("GET request to {Endpoint} completed successfully", endpoint);

      return content;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error making GET request to {Endpoint}", endpoint);
      throw;
    }
  }

  public async Task<string> PostAsync(string endpoint, string content, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrEmpty(endpoint))
      throw new ArgumentNullException(nameof(endpoint));
    if (content == null)
      throw new ArgumentNullException(nameof(content));

    try
    {
      _logger.LogInformation("Making POST request to {Endpoint}", endpoint);

      var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync(endpoint, httpContent, cancellationToken);
      response.EnsureSuccessStatusCode();

      var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

      _logger.LogInformation("POST request to {Endpoint} completed successfully", endpoint);

      return responseContent;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error making POST request to {Endpoint}", endpoint);
      throw;
    }
  }

  public async Task<string> PutAsync(string endpoint, string content, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrEmpty(endpoint))
      throw new ArgumentNullException(nameof(endpoint));
    if (content == null)
      throw new ArgumentNullException(nameof(content));

    try
    {
      _logger.LogInformation("Making PUT request to {Endpoint}", endpoint);

      var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
      var response = await _httpClient.PutAsync(endpoint, httpContent, cancellationToken);
      response.EnsureSuccessStatusCode();

      var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

      _logger.LogInformation("PUT request to {Endpoint} completed successfully", endpoint);

      return responseContent;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error making PUT request to {Endpoint}", endpoint);
      throw;
    }
  }

  public async Task<string> DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrEmpty(endpoint))
      throw new ArgumentNullException(nameof(endpoint));

    try
    {
      _logger.LogInformation("Making DELETE request to {Endpoint}", endpoint);

      var response = await _httpClient.DeleteAsync(endpoint, cancellationToken);
      response.EnsureSuccessStatusCode();

      var content = await response.Content.ReadAsStringAsync(cancellationToken);

      _logger.LogInformation("DELETE request to {Endpoint} completed successfully", endpoint);

      return content;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error making DELETE request to {Endpoint}", endpoint);
      throw;
    }
  }

  public void Dispose()
  {
    _httpClient?.Dispose();
  }
}