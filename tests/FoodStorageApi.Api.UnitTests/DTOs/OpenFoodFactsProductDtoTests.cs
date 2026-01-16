using FoodStorageApi.Api.DTOs.OpenFoodFacts;
using Xunit;

namespace FoodStorageApi.Api.UnitTests.DTOs;

public class OpenFoodFactsProductDtoTests
{
  [Fact]
  public void Properties_CanBeSetAndRetrieved()
  {
    // Arrange & Act
    var dto = new OpenFoodFactsProductDto
    {
      Code = "3017620422003",
      ProductName = "Nutella",
      Brands = "Ferrero",
      ImageUrl = "https://example.com/image.jpg",
      ServingQuantity = 15m,
      ServingQuantityUnit = "g",
      ImageIngredientsUrl = "https://example.com/ingredients.jpg",
      BrandList = new List<string> { "Ferrero" },
      ServingSize = "15 g",
      HasImage = true,
      HasIngredientsImage = true
    };

    // Assert
    Assert.Equal("3017620422003", dto.Code);
    Assert.Equal("Nutella", dto.ProductName);
    Assert.Equal("Ferrero", dto.Brands);
    Assert.Equal("https://example.com/image.jpg", dto.ImageUrl);
    Assert.Equal(15m, dto.ServingQuantity);
    Assert.Equal("g", dto.ServingQuantityUnit);
    Assert.Equal("https://example.com/ingredients.jpg", dto.ImageIngredientsUrl);
    Assert.Single(dto.BrandList);
    Assert.Contains("Ferrero", dto.BrandList);
    Assert.Equal("15 g", dto.ServingSize);
    Assert.True(dto.HasImage);
    Assert.True(dto.HasIngredientsImage);
  }

  [Fact]
  public void DefaultValues_AreCorrect()
  {
    // Act
    var dto = new OpenFoodFactsProductDto();

    // Assert
    Assert.Equal(string.Empty, dto.Code);
    Assert.Null(dto.ProductName);
    Assert.Null(dto.Brands);
    Assert.Null(dto.ImageUrl);
    Assert.Null(dto.ServingQuantity);
    Assert.Null(dto.ServingQuantityUnit);
    Assert.Null(dto.ImageIngredientsUrl);
    Assert.Empty(dto.BrandList);
    Assert.Equal(string.Empty, dto.ServingSize);
    Assert.False(dto.HasImage);
    Assert.False(dto.HasIngredientsImage);
  }
}