using System.Net;
using System.Text.Json;
using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Domain.Models.OpenFoodFacts;
using FoodStorageApi.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FoodStorageApi.Infrastructure.UnitTests.Services;

public class OpenFoodFactsServiceTests : IDisposable
{
  private readonly Mock<IBaseWebServiceClient> _mockWebServiceClient;
  private readonly Mock<ILogger<OpenFoodFactsService>> _mockLogger;
  private readonly OpenFoodFactsService _service;

  public OpenFoodFactsServiceTests()
  {
    _mockWebServiceClient = new Mock<IBaseWebServiceClient>();
    _mockLogger = new Mock<ILogger<OpenFoodFactsService>>();
    _service = new OpenFoodFactsService(_mockWebServiceClient.Object, _mockLogger.Object);
  }

  public void Dispose()
  {
    _service?.Dispose();
  }

  [Fact]
  public void Constructor_WithNullWebServiceClient_ThrowsArgumentNullException()
  {
    // Act & Assert
    Assert.Throws<ArgumentNullException>(() =>
        new OpenFoodFactsService(null!, _mockLogger.Object));
  }

  [Fact]
  public void Constructor_WithNullLogger_ThrowsArgumentNullException()
  {
    // Act & Assert
    Assert.Throws<ArgumentNullException>(() =>
        new OpenFoodFactsService(_mockWebServiceClient.Object, null!));
  }

  [Fact]
  public async Task GetProductByBarcodeAsync_WithNullBarcode_ThrowsArgumentException()
  {
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(() =>
        _service.GetProductByBarcodeAsync(null!));
  }

  [Fact]
  public async Task GetProductByBarcodeAsync_WithEmptyBarcode_ThrowsArgumentException()
  {
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(() =>
        _service.GetProductByBarcodeAsync(""));
  }

  [Fact]
  public async Task GetProductByBarcodeAsync_WithValidBarcode_ReturnsProduct()
  {
    // Arrange
    const string barcode = "3017620422003";
    const string jsonResponse = """
            {
                "code": "3017620422003",
                "product": {
                    "code": "3017620422003",
                    "product_name": "Nutella",
                    "brands": "Nutella,Ferrero",
                    "image_url": "https://example.com/image.jpg",
                    "serving_quantity": 15,
                    "serving_quantity_unit": "g"
                },
                "status": 1,
                "status_verbose": "product found"
            }
            """;

    _mockWebServiceClient
        .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(jsonResponse);

    // Act
    var result = await _service.GetProductByBarcodeAsync(barcode);

    // Assert
    Assert.NotNull(result);
    Assert.True(result.IsSuccess);
    Assert.Equal(barcode, result.Code);
    Assert.NotNull(result.Product);
    Assert.Equal("Nutella", result.Product.ProductName);
    Assert.Equal("Nutella,Ferrero", result.Product.Brands);
    Assert.Equal(15, result.Product.ServingQuantity);
    Assert.Equal("g", result.Product.ServingQuantityUnit);
  }

  [Fact]
  public async Task GetProductByBarcodeAsync_WithProductNotFound_ReturnsUnsuccessfulResponse()
  {
    // Arrange
    const string barcode = "9999999999999";
    const string jsonResponse = """
            {
                "code": "9999999999999",
                "status": 0,
                "status_verbose": "product not found"
            }
            """;

    _mockWebServiceClient
        .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(jsonResponse);

    // Act
    var result = await _service.GetProductByBarcodeAsync(barcode);

    // Assert
    Assert.NotNull(result);
    Assert.False(result.IsSuccess);
    Assert.Equal("product not found", result.StatusVerbose);
  }

  [Fact]
  public async Task GetProductByBarcodeAsync_WithEmptyResponse_ReturnsNull()
  {
    // Arrange
    const string barcode = "3017620422003";

    _mockWebServiceClient
        .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync("");

    // Act
    var result = await _service.GetProductByBarcodeAsync(barcode);

    // Assert
    Assert.Null(result);
  }

  [Fact]
  public async Task SearchProductsByNameAsync_WithNullProductName_ThrowsArgumentException()
  {
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(() =>
        _service.SearchProductsByNameAsync(null!));
  }

  [Fact]
  public async Task SearchProductsByNameAsync_WithEmptyProductName_ThrowsArgumentException()
  {
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(() =>
        _service.SearchProductsByNameAsync(""));
  }

  [Fact]
  public async Task SearchProductsByNameAsync_WithInvalidPageSize_ThrowsArgumentOutOfRangeException()
  {
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
        _service.SearchProductsByNameAsync("Nutella", pageSize: 0));

    await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
        _service.SearchProductsByNameAsync("Nutella", pageSize: 101));
  }

  [Fact]
  public async Task SearchProductsByNameAsync_WithInvalidPage_ThrowsArgumentOutOfRangeException()
  {
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
        _service.SearchProductsByNameAsync("Nutella", page: 0));
  }

  [Fact]
  public async Task SearchProductsByNameAsync_WithValidParameters_ReturnsProducts()
  {
    // Arrange
    const string productName = "Nutella";
    const string jsonResponse = """
            {
                "products": [
                    {
                        "code": "3017620422003",
                        "product_name": "Nutella",
                        "brands": "Nutella,Ferrero",
                        "serving_quantity": 15,
                        "serving_quantity_unit": "g"
                    }
                ],
                "count": 1,
                "page": 1,
                "page_size": 20
            }
            """;

    _mockWebServiceClient
        .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(jsonResponse);

    // Act
    var result = await _service.SearchProductsByNameAsync(productName);

    // Assert
    Assert.NotNull(result);
    var products = result.ToList();
    Assert.Single(products);
    Assert.Equal("Nutella", products[0].ProductName);
    Assert.Equal("3017620422003", products[0].Code);
  }

  [Fact]
  public async Task SearchProductsByNameAsync_WithEmptyResponse_ReturnsEmptyList()
  {
    // Arrange
    const string productName = "NonExistentProduct";

    _mockWebServiceClient
        .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync("");

    // Act
    var result = await _service.SearchProductsByNameAsync(productName);

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
  }

  [Fact]
  public async Task GetProductByBarcodeAsync_CallsCorrectEndpoint()
  {
    // Arrange
    const string barcode = "3017620422003";
    const string expectedEndpoint = "https://world.openfoodfacts.org/api/v2/product/3017620422003.json?fields=code,product_name,brands,image_url,serving_quantity,serving_quantity_unit,image_ingredients_url";

    _mockWebServiceClient
        .Setup(x => x.GetAsync(expectedEndpoint, It.IsAny<CancellationToken>()))
        .ReturnsAsync("{}");

    // Act
    await _service.GetProductByBarcodeAsync(barcode);

    // Assert
    _mockWebServiceClient.Verify(
        x => x.GetAsync(expectedEndpoint, It.IsAny<CancellationToken>()),
        Times.Once);
  }
}