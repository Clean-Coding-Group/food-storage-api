using FoodStorageApi.Domain.Models.OpenFoodFacts;
using Xunit;

namespace FoodStorageApi.Infrastructure.UnitTests.Models;

public class OpenFoodFactsApiResponseTests
{
  [Fact]
  public void IsSuccess_WithStatus1_ReturnsTrue()
  {
    // Arrange
    var response = new OpenFoodFactsApiResponse
    {
      Status = 1,
      StatusVerbose = "product found"
    };

    // Act & Assert
    Assert.True(response.IsSuccess);
  }

  [Fact]
  public void IsSuccess_WithStatus0_ReturnsFalse()
  {
    // Arrange
    var response = new OpenFoodFactsApiResponse
    {
      Status = 0,
      StatusVerbose = "product not found"
    };

    // Act & Assert
    Assert.False(response.IsSuccess);
  }

  [Fact]
  public void Properties_CanBeSetAndRetrieved()
  {
    // Arrange
    const string code = "3017620422003";
    const string statusVerbose = "product found";
    var product = new OpenFoodFactsProduct { Code = code };

    // Act
    var response = new OpenFoodFactsApiResponse
    {
      Code = code,
      Product = product,
      Status = 1,
      StatusVerbose = statusVerbose
    };

    // Assert
    Assert.Equal(code, response.Code);
    Assert.Equal(product, response.Product);
    Assert.Equal(1, response.Status);
    Assert.Equal(statusVerbose, response.StatusVerbose);
    Assert.True(response.IsSuccess);
  }
}