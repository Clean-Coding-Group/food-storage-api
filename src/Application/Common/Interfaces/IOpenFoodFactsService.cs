using FoodStorageApi.Domain.Models.OpenFoodFacts;

namespace FoodStorageApi.Application.Common.Interfaces;

/// <summary>
/// Service interface for interacting with the OpenFoodFacts API
/// </summary>
public interface IOpenFoodFactsService
{
  /// <summary>
  /// Retrieves product information from OpenFoodFacts by barcode
  /// </summary>
  /// <param name="barcode">The product barcode/code</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <returns>OpenFoodFacts API response containing product information</returns>
  Task<OpenFoodFactsApiResponse?> GetProductByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);

  /// <summary>
  /// Searches for products by name
  /// </summary>
  /// <param name="productName">The product name to search for</param>
  /// <param name="pageSize">Number of results per page (default: 20, max: 100)</param>
  /// <param name="page">Page number (default: 1)</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <returns>List of matching products</returns>
  Task<IEnumerable<OpenFoodFactsProduct>> SearchProductsByNameAsync(
      string productName,
      int page = 1,
      int pageSize = 20,
      CancellationToken cancellationToken = default);
}