using FoodStorageApi.Api.DTOs.OpenFoodFacts;
using Xunit;

namespace FoodStorageApi.Api.UnitTests.DTOs;

public class OpenFoodFactsApiResponseDtoTests
{
  [Fact]
  public void IsSuccess_WithStatus1_ReturnsTrue()
  {
    // Arrange
    var dto = new OpenFoodFactsApiResponseDto { Status = 1 };

    // Act & Assert
    Assert.True(dto.IsSuccess);
  }

  [Fact]
  public void IsSuccess_WithStatus0_ReturnsFalse()
  {
    // Arrange
    var dto = new OpenFoodFactsApiResponseDto { Status = 0 };

    // Act & Assert
    Assert.False(dto.IsSuccess);
  }

  [Fact]
  public void Properties_CanBeSetAndRetrieved()
  {
    // Arrange
    var product = new OpenFoodFactsProductDto
    {
      Code = "3017620422003",
      ProductName = "Nutella"
    };

    // Act
    var dto = new OpenFoodFactsApiResponseDto
    {
      Status = 1,
      Product = product
    };

    // Assert
    Assert.Equal(1, dto.Status);
    Assert.NotNull(dto.Product);
    Assert.Equal("3017620422003", dto.Product.Code);
    Assert.Equal("Nutella", dto.Product.ProductName);
    Assert.True(dto.IsSuccess);
  }

  [Fact]
  public void DefaultValues_AreCorrect()
  {
    // Act
    var dto = new OpenFoodFactsApiResponseDto();

    // Assert
    Assert.Equal(0, dto.Status);
    Assert.Null(dto.Product);
    Assert.False(dto.IsSuccess);
  }
}