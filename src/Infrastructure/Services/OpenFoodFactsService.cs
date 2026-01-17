using System.Text.Json;
using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Domain.Models.OpenFoodFacts;
using Microsoft.Extensions.Logging;

namespace FoodStorageApi.Infrastructure.Services;

/// <summary>
/// Service for interacting with the OpenFoodFacts API
/// </summary>
public partial class OpenFoodFactsService : IOpenFoodFactsService, IDisposable
{
  private readonly IBaseWebServiceClient _webServiceClient;
  private readonly ILogger<OpenFoodFactsService> _logger;
  private readonly JsonSerializerOptions _jsonOptions;
  private const string BaseUrl = "https://world.openfoodfacts.org/api/v2";
  private const string ProductFields = "code,product_name,brands,image_url,serving_quantity,serving_quantity_unit,image_ingredients_url";

  public OpenFoodFactsService(IBaseWebServiceClient webServiceClient, ILogger<OpenFoodFactsService> logger)
  {
    _webServiceClient = webServiceClient ?? throw new ArgumentNullException(nameof(webServiceClient));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    _jsonOptions = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };
  }

  /// <summary>
  /// Retrieves product information from OpenFoodFacts by barcode
  /// </summary>
  public async Task<OpenFoodFactsApiResponse?> GetProductByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(barcode))
      throw new ArgumentException("Barcode cannot be null or empty", nameof(barcode));

    try
    {
      _logger.LogInformation("Retrieving product information for barcode: {Barcode}", barcode);

      var endpoint = $"{BaseUrl}/product/{barcode}.json?fields={ProductFields}";
      var response = await _webServiceClient.GetAsync(endpoint, cancellationToken);

      if (string.IsNullOrWhiteSpace(response))
      {
        _logger.LogWarning("Empty response received for barcode: {Barcode}", barcode);
        return null;
      }

      var result = JsonSerializer.Deserialize<OpenFoodFactsApiResponse>(response, _jsonOptions);

      if (result?.IsSuccess == true)
      {
        _logger.LogInformation("Successfully retrieved product: {ProductName} for barcode: {Barcode}",
            result.Product?.ProductName, barcode);
      }
      else
      {
        _logger.LogWarning("Product not found for barcode: {Barcode}. Status: {Status}",
            barcode, result?.StatusVerbose);
      }

      return result;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error retrieving product information for barcode: {Barcode}", barcode);
      throw;
    }
  }

  /// <summary>
  /// Searches for products by name
  /// </summary>
  public async Task<IEnumerable<OpenFoodFactsProduct>> SearchProductsByNameAsync(
      string productName,
      int pageSize = 20,
      int page = 1,
      CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(productName))
      throw new ArgumentException("Product name cannot be null or empty", nameof(productName));

    if (pageSize <= 0 || pageSize > 100)
      throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 100");

    if (page <= 0)
      throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0");

    try
    {
      _logger.LogInformation("Searching products by name: {ProductName}, page: {Page}, size: {PageSize}",
          productName, page, pageSize);

      var encodedName = Uri.EscapeDataString(productName);
      var endpoint = $"{BaseUrl}/search?search_terms={encodedName}&page={page}&page_size={pageSize}&fields={ProductFields}&json=true";

      var response = await _webServiceClient.GetAsync(endpoint, cancellationToken);

      if (string.IsNullOrWhiteSpace(response))
      {
        _logger.LogWarning("Empty response received for product search: {ProductName}", productName);
        return Enumerable.Empty<OpenFoodFactsProduct>();
      }

      var searchResult = JsonSerializer.Deserialize<OpenFoodFactsSearchResponse>(response, _jsonOptions);

      var products = searchResult?.Products ?? Enumerable.Empty<OpenFoodFactsProduct>();

      _logger.LogInformation("Found {Count} products for search: {ProductName}",
          products.Count(), productName);

      return products;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error searching products by name: {ProductName}", productName);
      throw;
    }
  }

  public void Dispose()
  {
    // The web service client will be disposed by DI container
    // This method is here to implement IDisposable for consistency
  }
}