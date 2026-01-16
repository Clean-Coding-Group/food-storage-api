using System.Text.Json.Serialization;

namespace FoodStorageApi.Domain.Models.OpenFoodFacts;

/// <summary>
/// Represents the root response from the OpenFoodFacts API
/// </summary>
public class OpenFoodFactsApiResponse
{
  [JsonPropertyName("code")]
  public string Code { get; set; } = string.Empty;

  [JsonPropertyName("product")]
  public OpenFoodFactsProduct? Product { get; set; }

  [JsonPropertyName("status")]
  public int Status { get; set; }

  [JsonPropertyName("status_verbose")]
  public string StatusVerbose { get; set; } = string.Empty;

  /// <summary>
  /// Indicates if the product was found successfully
  /// </summary>
  public bool IsSuccess => Status == 1;
}