using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FoodStorageApi.Domain.Models.OpenFoodFacts;

/// <summary>
/// Represents a product from the OpenFoodFacts API
/// </summary>
public class OpenFoodFactsProduct
{
  [JsonPropertyName("code")]
  [Required]
  public string Code { get; set; } = string.Empty;

  [JsonPropertyName("product_name")]
  public string? ProductName { get; set; }

  [JsonPropertyName("brands")]
  public string? Brands { get; set; }

  [JsonPropertyName("image_url")]
  public string? ImageUrl { get; set; }

  [JsonPropertyName("image_ingredients_url")]
  public string? ImageIngredientsUrl { get; set; }

  [JsonPropertyName("serving_quantity")]
  public decimal? ServingQuantity { get; set; }

  [JsonPropertyName("serving_quantity_unit")]
  public string? ServingQuantityUnit { get; set; }

  /// <summary>
  /// Gets a list of individual brands from the brands string (comma-separated)
  /// </summary>
  public IEnumerable<string> BrandList
  {
    get
    {
      if (string.IsNullOrWhiteSpace(Brands))
        return Enumerable.Empty<string>();

      return Brands.Split(',', StringSplitOptions.RemoveEmptyEntries)
                  .Select(b => b.Trim())
                  .Where(b => !string.IsNullOrEmpty(b));
    }
  }

  /// <summary>
  /// Gets the serving size as a formatted string
  /// </summary>
  public string ServingSize
  {
    get
    {
      if (ServingQuantity.HasValue && !string.IsNullOrWhiteSpace(ServingQuantityUnit))
        return $"{ServingQuantity.Value} {ServingQuantityUnit}";

      return string.Empty;
    }
  }

  /// <summary>
  /// Indicates if the product has a valid image URL
  /// </summary>
  public bool HasImage => !string.IsNullOrWhiteSpace(ImageUrl);

  /// <summary>
  /// Indicates if the product has ingredients image URL
  /// </summary>
  public bool HasIngredientsImage => !string.IsNullOrWhiteSpace(ImageIngredientsUrl);
}