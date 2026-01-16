using System.ComponentModel.DataAnnotations;

namespace FoodStorageApi.Api.DTOs.OpenFoodFacts;

/// <summary>
/// Data Transfer Object for OpenFoodFacts API response
/// </summary>
public class OpenFoodFactsApiResponseDto
{
  /// <summary>
  /// API response status (1 = success, 0 = failure)
  /// </summary>
  [Required]
  public int Status { get; set; }

  /// <summary>
  /// Product information (null if not found)
  /// </summary>
  public OpenFoodFactsProductDto? Product { get; set; }

  /// <summary>
  /// Indicates whether the API response was successful
  /// </summary>
  public bool IsSuccess => Status == 1;
}