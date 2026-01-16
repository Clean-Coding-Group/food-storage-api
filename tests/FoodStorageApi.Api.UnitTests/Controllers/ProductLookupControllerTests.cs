using AutoMapper;
using FoodStorageApi.Api.Controllers;
using FoodStorageApi.Api.DTOs.OpenFoodFacts;
using FoodStorageApi.Api.Mapping;
using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Domain.Models.OpenFoodFacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FoodStorageApi.Api.UnitTests.Controllers;

public class ProductLookupControllerTests
{
  private readonly Mock<IOpenFoodFactsService> _mockService;
  private readonly Mock<ILogger<ProductLookupController>> _mockLogger;
  private readonly IMapper _mapper;
  private readonly ProductLookupController _controller;

  public ProductLookupControllerTests()
  {
    _mockService = new Mock<IOpenFoodFactsService>();
    _mockLogger = new Mock<ILogger<ProductLookupController>>();

    var config = new MapperConfiguration(cfg =>
    {
      cfg.AddProfile<OpenFoodFactsMappingProfile>();
    });
    _mapper = config.CreateMapper();

    _controller = new ProductLookupController(_mockService.Object, _mapper, _mockLogger.Object);
  }

  [Fact]
  public async Task GetProductByBarcode_WithValidBarcode_ReturnsOkWithDto()
  {
    // Arrange
    var barcode = "3017620422003";
    var product = new OpenFoodFactsProduct
    {
      Code = barcode,
      ProductName = "Nutella",
      Brands = "Ferrero",
      ImageUrl = "https://example.com/image.jpg"
    };
    var apiResponse = new OpenFoodFactsApiResponse
    {
      Status = 1,
      Product = product
    };

    _mockService.Setup(s => s.GetProductByBarcodeAsync(barcode, It.IsAny<CancellationToken>()))
        .ReturnsAsync(apiResponse);

    // Act
    var result = await _controller.GetProductByBarcode(barcode);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var dto = Assert.IsType<OpenFoodFactsApiResponseDto>(okResult.Value);
    Assert.Equal(apiResponse.Status, dto.Status);
    Assert.True(dto.IsSuccess);
    Assert.NotNull(dto.Product);
    Assert.Equal(product.Code, dto.Product.Code);
    Assert.Equal(product.ProductName, dto.Product.ProductName);
  }

  [Fact]
  public async Task GetProductByBarcode_WithProductNotFound_ReturnsNotFound()
  {
    // Arrange
    var barcode = "9999999999999";
    var apiResponse = new OpenFoodFactsApiResponse
    {
      Status = 0,
      Product = null
    };

    _mockService.Setup(s => s.GetProductByBarcodeAsync(barcode, It.IsAny<CancellationToken>()))
        .ReturnsAsync(apiResponse);

    // Act
    var result = await _controller.GetProductByBarcode(barcode);

    // Assert
    Assert.IsType<NotFoundObjectResult>(result.Result);
  }

  [Theory]
  [InlineData("")]
  [InlineData("   ")]
  [InlineData(null)]
  public async Task GetProductByBarcode_WithInvalidBarcode_ReturnsBadRequest(string barcode)
  {
    // Act
    var result = await _controller.GetProductByBarcode(barcode);

    // Assert
    Assert.IsType<BadRequestObjectResult>(result.Result);
  }

  [Fact]
  public async Task SearchProducts_WithValidQuery_ReturnsOkWithDtos()
  {
    // Arrange
    var query = "Nutella";
    var products = new List<OpenFoodFactsProduct>
        {
            new()
            {
                Code = "3017620422003",
                ProductName = "Nutella",
                Brands = "Ferrero"
            },
            new()
            {
                Code = "8000500372456",
                ProductName = "Nutella B-ready",
                Brands = "Ferrero"
            }
        };

    _mockService.Setup(s => s.SearchProductsByNameAsync(query, 20, 1, It.IsAny<CancellationToken>()))
        .ReturnsAsync(products);

    // Act
    var result = await _controller.SearchProducts(query);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var dtos = Assert.IsType<List<OpenFoodFactsProductDto>>(okResult.Value);
    Assert.Equal(2, dtos.Count);
    Assert.Equal(products[0].Code, dtos[0].Code);
    Assert.Equal(products[0].ProductName, dtos[0].ProductName);
    Assert.Equal(products[1].Code, dtos[1].Code);
    Assert.Equal(products[1].ProductName, dtos[1].ProductName);
  }

  [Theory]
  [InlineData("")]
  [InlineData("   ")]
  [InlineData(null)]
  public async Task SearchProducts_WithInvalidQuery_ReturnsBadRequest(string query)
  {
    // Act
    var result = await _controller.SearchProducts(query);

    // Assert
    Assert.IsType<BadRequestObjectResult>(result.Result);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  [InlineData(101)]
  public async Task SearchProducts_WithInvalidPageSize_ReturnsBadRequest(int pageSize)
  {
    // Act
    var result = await _controller.SearchProducts("query", pageSize);

    // Assert
    Assert.IsType<BadRequestObjectResult>(result.Result);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public async Task SearchProducts_WithInvalidPage_ReturnsBadRequest(int page)
  {
    // Act
    var result = await _controller.SearchProducts("query", 20, page);

    // Assert
    Assert.IsType<BadRequestObjectResult>(result.Result);
  }

  [Fact]
  public void GetApiInfo_ReturnsOkWithInformation()
  {
    // Act
    var result = _controller.GetApiInfo();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    Assert.NotNull(okResult.Value);
  }

  [Fact]
  public async Task GetProductByBarcode_WithServiceException_ReturnsInternalServerError()
  {
    // Arrange
    var barcode = "3017620422003";
    _mockService.Setup(s => s.GetProductByBarcodeAsync(barcode, It.IsAny<CancellationToken>()))
        .ThrowsAsync(new Exception("Service error"));

    // Act
    var result = await _controller.GetProductByBarcode(barcode);

    // Assert
    var statusResult = Assert.IsType<ObjectResult>(result.Result);
    Assert.Equal(500, statusResult.StatusCode);
  }

  [Fact]
  public async Task SearchProducts_WithServiceException_ReturnsInternalServerError()
  {
    // Arrange
    var query = "Nutella";
    _mockService.Setup(s => s.SearchProductsByNameAsync(query, 20, 1, It.IsAny<CancellationToken>()))
        .ThrowsAsync(new Exception("Service error"));

    // Act
    var result = await _controller.SearchProducts(query);

    // Assert
    var statusResult = Assert.IsType<ObjectResult>(result.Result);
    Assert.Equal(500, statusResult.StatusCode);
  }
}