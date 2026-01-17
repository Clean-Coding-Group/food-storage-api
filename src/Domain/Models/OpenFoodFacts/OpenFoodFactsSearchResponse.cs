using FoodStorageApi.Domain.Models.OpenFoodFacts;

namespace FoodStorageApi.Domain.Models.OpenFoodFacts;

public class OpenFoodFactsSearchResponse
{
  [System.Text.Json.Serialization.JsonPropertyName("products")]
  public List<OpenFoodFactsProduct> Products { get; set; } = new();

  [System.Text.Json.Serialization.JsonPropertyName("count")]
  public int Count { get; set; }

  [System.Text.Json.Serialization.JsonPropertyName("page")]
  public int Page { get; set; }

  [System.Text.Json.Serialization.JsonPropertyName("page_size")]
  public int PageSize { get; set; }
}