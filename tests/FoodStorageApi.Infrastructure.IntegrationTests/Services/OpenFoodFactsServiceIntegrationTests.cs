using System.Net;
using System.Text.Json;
using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Domain.Models.OpenFoodFacts;
using FoodStorageApi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FoodStorageApi.Infrastructure.IntegrationTests.Services;

public class OpenFoodFactsServiceIntegrationTests : IDisposable
{
  private readonly HttpClient _httpClient;
  private readonly OpenFoodFactsService _openFoodFactsService;

  public OpenFoodFactsServiceIntegrationTests()
  {
    // Use a simple HttpClient for testing
    _httpClient = new HttpClient();

    // Create a logger
    var services = new ServiceCollection()
        .AddLogging(builder => builder.AddConsole())
        .BuildServiceProvider();

    var logger = services.GetRequiredService<ILogger<OpenFoodFactsService>>();
    var baseWebServiceClient = new BaseWebServiceClient(_httpClient, services.GetRequiredService<ILogger<BaseWebServiceClient>>());
    _openFoodFactsService = new OpenFoodFactsService(baseWebServiceClient, logger);
  }

  public void Dispose()
  {
    _httpClient?.Dispose();
    _openFoodFactsService?.Dispose();
  }

  [Fact]
  public async Task GetProductByBarcodeAsync_WithKnownNutellaBarcode_ReturnsValidProduct()
  {
    // Arrange - Using known Nutella barcode
    const string nutellaBarcode = "3017620422003";

    // Act
    var result = await _openFoodFactsService.GetProductByBarcodeAsync(nutellaBarcode);

    // Assert
    Assert.NotNull(result);
    Assert.True(result.IsSuccess);
    Assert.Equal(nutellaBarcode, result.Code);
    Assert.NotNull(result.Product);
    Assert.False(string.IsNullOrEmpty(result.Product.ProductName));

    // Product should have basic information
    Assert.NotNull(result.Product.Code);
    Assert.Equal(nutellaBarcode, result.Product.Code);

    // Should have serving information if available
    if (result.Product.ServingQuantity.HasValue)
    {
      Assert.True(result.Product.ServingQuantity.Value > 0);
    }
  }

  [Fact]
  public async Task GetProductByBarcodeAsync_WithInvalidBarcode_ReturnsUnsuccessfulResponse()
  {
    // Arrange - Using a barcode that doesn't exist
    const string invalidBarcode = "9999999999999";

    // Act
    var result = await _openFoodFactsService.GetProductByBarcodeAsync(invalidBarcode);

    // Assert
    Assert.NotNull(result);
    Assert.False(result.IsSuccess);
  }

  [Fact]
  public async Task SearchProductsByNameAsync_WithNutella_ReturnsResults()
  {
    // Arrange
    const string searchTerm = "Nutella";

    // Act
    var results = await _openFoodFactsService.SearchProductsByNameAsync(searchTerm, pageSize: 5);

    // Assert
    Assert.NotNull(results);
    var productsList = results.ToList();

    // Should return some results for a popular product like Nutella
    Assert.NotEmpty(productsList);
    Assert.True(productsList.Count <= 5); // Respects page size

    // At least one result should contain Nutella in name or brands
    Assert.Contains(productsList, p =>
        (p.ProductName?.Contains("Nutella", StringComparison.OrdinalIgnoreCase) == true) ||
        (p.Brands?.Contains("Nutella", StringComparison.OrdinalIgnoreCase) == true));
  }

  [Fact]
  public async Task SearchProductsByNameAsync_WithVeryUnlikelySearchTerm_ReturnsEmptyResults()
  {
    // Arrange
    const string searchTerm = "XYZ123NonExistentProduct456";

    // Act
    var results = await _openFoodFactsService.SearchProductsByNameAsync(searchTerm);

    // Assert
    Assert.NotNull(results);
    // Should return empty results for a non-existent product
    Assert.Empty(results);
  }

  [Fact]
  public async Task GetProductByBarcodeAsync_VerifiesJsonDeserialization()
  {
    // Arrange
    const string nutellaBarcode = "3017620422003";

    // Act
    var result = await _openFoodFactsService.GetProductByBarcodeAsync(nutellaBarcode);

    // Assert
    Assert.NotNull(result);

    if (result.IsSuccess && result.Product != null)
    {
      // Verify that all expected properties can be accessed
      var product = result.Product;

      // These should not throw exceptions
      _ = product.Code;
      _ = product.ProductName;
      _ = product.Brands;
      _ = product.ImageUrl;
      _ = product.ImageIngredientsUrl;
      _ = product.ServingQuantity;
      _ = product.ServingQuantityUnit;

      // Test computed properties
      _ = product.BrandList.ToList();
      _ = product.ServingSize;
      _ = product.HasImage;
      _ = product.HasIngredientsImage;

      // Code should always be available and valid
      Assert.NotNull(product.Code);
      Assert.NotEmpty(product.Code);
    }
  }

  [Fact]
  public async Task OpenFoodFactsService_HandlesNetworkTimeout()
  {
    // Arrange
    using var timeoutHttpClient = new HttpClient();
    timeoutHttpClient.Timeout = TimeSpan.FromMilliseconds(1); // Very short timeout

    var services = new ServiceCollection()
        .AddLogging(builder => builder.AddConsole())
        .BuildServiceProvider();

    var logger = services.GetRequiredService<ILogger<OpenFoodFactsService>>();
    var baseWebServiceClient = new BaseWebServiceClient(timeoutHttpClient, services.GetRequiredService<ILogger<BaseWebServiceClient>>());
    var timeoutService = new OpenFoodFactsService(baseWebServiceClient, logger);

    // Act & Assert
    await Assert.ThrowsAnyAsync<Exception>(async () =>
    {
      await timeoutService.GetProductByBarcodeAsync("3017620422003");
    });

    timeoutService.Dispose();
  }
}