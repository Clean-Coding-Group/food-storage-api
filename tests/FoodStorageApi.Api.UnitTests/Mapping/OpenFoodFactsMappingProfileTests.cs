using AutoMapper;
using FoodStorageApi.Api.DTOs.OpenFoodFacts;
using FoodStorageApi.Api.Mapping;
using FoodStorageApi.Domain.Models.OpenFoodFacts;
using Xunit;

namespace FoodStorageApi.Api.UnitTests.Mapping;

public class OpenFoodFactsMappingProfileTests
{
  private readonly IMapper _mapper;

  public OpenFoodFactsMappingProfileTests()
  {
    var config = new MapperConfiguration(cfg =>
    {
      cfg.AddProfile<OpenFoodFactsMappingProfile>();
    });
    _mapper = config.CreateMapper();
  }

  [Fact]
  public void Should_Map_OpenFoodFactsProduct_To_OpenFoodFactsProductDto()
  {
    // Arrange
    var product = new OpenFoodFactsProduct
    {
      Code = "3017620422003",
      ProductName = "Nutella",
      Brands = "Ferrero",
      ImageUrl = "https://example.com/image.jpg",
      ServingQuantity = 15m,
      ServingQuantityUnit = "g",
      ImageIngredientsUrl = "https://example.com/ingredients.jpg"
    };

    // Act
    var dto = _mapper.Map<OpenFoodFactsProductDto>(product);

    // Assert
    Assert.NotNull(dto);
    Assert.Equal(product.Code, dto.Code);
    Assert.Equal(product.ProductName, dto.ProductName);
    Assert.Equal(product.Brands, dto.Brands);
    Assert.Equal(product.ImageUrl, dto.ImageUrl);
    Assert.Equal(product.ServingQuantity, dto.ServingQuantity);
    Assert.Equal(product.ServingQuantityUnit, dto.ServingQuantityUnit);
    Assert.Equal(product.ImageIngredientsUrl, dto.ImageIngredientsUrl);
    Assert.Equal(product.BrandList, dto.BrandList);
    Assert.Equal(product.ServingSize, dto.ServingSize);
    Assert.Equal(product.HasImage, dto.HasImage);
    Assert.Equal(product.HasIngredientsImage, dto.HasIngredientsImage);
  }

  [Fact]
  public void Should_Map_OpenFoodFactsApiResponse_To_OpenFoodFactsApiResponseDto()
  {
    // Arrange
    var response = new OpenFoodFactsApiResponse
    {
      Status = 1,
      Product = new OpenFoodFactsProduct
      {
        Code = "3017620422003",
        ProductName = "Nutella",
        Brands = "Ferrero"
      }
    };

    // Act
    var dto = _mapper.Map<OpenFoodFactsApiResponseDto>(response);

    // Assert
    Assert.NotNull(dto);
    Assert.Equal(response.Status, dto.Status);
    Assert.True(dto.IsSuccess);
    Assert.NotNull(dto.Product);
    Assert.Equal(response.Product.Code, dto.Product.Code);
    Assert.Equal(response.Product.ProductName, dto.Product.ProductName);
    Assert.Equal(response.Product.Brands, dto.Product.Brands);
  }

  [Fact]
  public void Should_Map_UnsuccessfulApiResponse_To_Dto()
  {
    // Arrange
    var response = new OpenFoodFactsApiResponse
    {
      Status = 0,
      Product = null
    };

    // Act
    var dto = _mapper.Map<OpenFoodFactsApiResponseDto>(response);

    // Assert
    Assert.NotNull(dto);
    Assert.Equal(response.Status, dto.Status);
    Assert.False(dto.IsSuccess);
    Assert.Null(dto.Product);
  }
}