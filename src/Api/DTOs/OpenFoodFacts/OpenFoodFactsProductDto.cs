using System.ComponentModel.DataAnnotations;

namespace FoodStorageApi.Api.DTOs.OpenFoodFacts;

/// <summary>
/// Data Transfer Object for OpenFoodFacts product information
/// </summary>
public class OpenFoodFactsProductDto
{
  /// <summary>
  /// Product barcode/identifier
  /// </summary>
  [Required]
  public string Code { get; set; } = string.Empty;

  /// <summary>
  /// Product name
  /// </summary>
  public string? ProductName { get; set; }

  /// <summary>
  /// Comma-separated list of brands
  /// </summary>
  public string? Brands { get; set; }

  /// <summary>
  /// Main product image URL
  /// </summary>
  public string? ImageUrl { get; set; }

  /// <summary>
  /// Serving quantity (numeric value)
  /// </summary>
  public decimal? ServingQuantity { get; set; }
  /// </summary>
  public string? ServingQuantityUnit { get; set; }

  /// <summary>
  /// Ingredients image URL
  /// </summary>
  public string? ImageIngredientsUrl { get; set; }

  /// <summary>
  /// List of individual brands (computed from comma-separated brands string)
  /// </summary>
  public List<string> BrandList { get; set; } = new();

  /// <summary>
  /// Formatted serving size string (e.g., "150 g")
  /// </summary>
  public string ServingSize { get; set; } = string.Empty;

  /// <summary>
  /// Indicates whether the product has a main image
  /// </summary>
  public bool HasImage { get; set; }

  /// <summary>
  /// Indicates whether the product has an ingredients image
  /// </summary>
  public bool HasIngredientsImage { get; set; }
}