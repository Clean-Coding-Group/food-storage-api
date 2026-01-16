using FoodStorageApi.Domain.Models.OpenFoodFacts;
using Xunit;

namespace FoodStorageApi.Infrastructure.UnitTests.Models;

public class OpenFoodFactsProductTests
{
  [Fact]
  public void BrandList_WithCommaSeparatedBrands_ReturnsIndividualBrands()
  {
    // Arrange
    var product = new OpenFoodFactsProduct
    {
      Brands = "Nutella,Ferrero"
    };

    // Act
    var brandList = product.BrandList.ToList();

    // Assert
    Assert.Equal(2, brandList.Count);
    Assert.Contains("Nutella", brandList);
    Assert.Contains("Ferrero", brandList);
  }

  [Fact]
  public void BrandList_WithEmptyBrands_ReturnsEmptyList()
  {
    // Arrange
    var product = new OpenFoodFactsProduct
    {
      Brands = ""
    };

    // Act
    var brandList = product.BrandList.ToList();

    // Assert
    Assert.Empty(brandList);
  }

  [Fact]
  public void BrandList_WithNullBrands_ReturnsEmptyList()
  {
    // Arrange
    var product = new OpenFoodFactsProduct
    {
      Brands = null
    };

    // Act
    var brandList = product.BrandList.ToList();

    // Assert
    Assert.Empty(brandList);
  }

  [Fact]
  public void ServingSize_WithQuantityAndUnit_ReturnsFormattedString()
  {
    // Arrange
    var product = new OpenFoodFactsProduct
    {
      ServingQuantity = 15,
      ServingQuantityUnit = "g"
    };

    // Act
    var servingSize = product.ServingSize;

    // Assert
    Assert.Equal("15 g", servingSize);
  }

  [Fact]
  public void ServingSize_WithoutQuantity_ReturnsEmptyString()
  {
    // Arrange
    var product = new OpenFoodFactsProduct
    {
      ServingQuantity = null,
      ServingQuantityUnit = "g"
    };

    // Act
    var servingSize = product.ServingSize;

    // Assert
    Assert.Equal(string.Empty, servingSize);
  }

  [Fact]
  public void HasImage_WithImageUrl_ReturnsTrue()
  {
    // Arrange
    var product = new OpenFoodFactsProduct
    {
      ImageUrl = "https://example.com/image.jpg"
    };

    // Act & Assert
    Assert.True(product.HasImage);
  }

  [Fact]
  public void HasImage_WithoutImageUrl_ReturnsFalse()
  {
    // Arrange
    var product = new OpenFoodFactsProduct
    {
      ImageUrl = null
    };

    // Act & Assert
    Assert.False(product.HasImage);
  }

  [Fact]
  public void HasIngredientsImage_WithIngredientsUrl_ReturnsTrue()
  {
    // Arrange
    var product = new OpenFoodFactsProduct
    {
      ImageIngredientsUrl = "https://example.com/ingredients.jpg"
    };

    // Act & Assert
    Assert.True(product.HasIngredientsImage);
  }

  [Fact]
  public void HasIngredientsImage_WithoutIngredientsUrl_ReturnsFalse()
  {
    // Arrange
    var product = new OpenFoodFactsProduct
    {
      ImageIngredientsUrl = ""
    };

    // Act & Assert
    Assert.False(product.HasIngredientsImage);
  }
}