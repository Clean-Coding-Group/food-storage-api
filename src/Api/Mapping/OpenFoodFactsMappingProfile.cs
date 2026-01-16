using AutoMapper;
using FoodStorageApi.Api.DTOs.OpenFoodFacts;
using FoodStorageApi.Domain.Models.OpenFoodFacts;

namespace FoodStorageApi.Api.Mapping;

/// <summary>
/// AutoMapper profile for OpenFoodFacts entity mappings
/// </summary>
public class OpenFoodFactsMappingProfile : Profile
{
  public OpenFoodFactsMappingProfile()
  {
    CreateMap<OpenFoodFactsProduct, OpenFoodFactsProductDto>()
        .ForMember(dest => dest.BrandList, opt => opt.MapFrom(src => src.BrandList))
        .ForMember(dest => dest.ServingSize, opt => opt.MapFrom(src => src.ServingSize))
        .ForMember(dest => dest.HasImage, opt => opt.MapFrom(src => src.HasImage))
        .ForMember(dest => dest.HasIngredientsImage, opt => opt.MapFrom(src => src.HasIngredientsImage));

    CreateMap<OpenFoodFactsApiResponse, OpenFoodFactsApiResponseDto>();
  }
}