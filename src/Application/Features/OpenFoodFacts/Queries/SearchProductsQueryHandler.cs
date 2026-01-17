using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Domain.Models.OpenFoodFacts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FoodStorageApi.Application.Features.OpenFoodFacts.Queries;

/// <summary>
/// Handler for SearchProductsQuery
/// </summary>
public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, IEnumerable<OpenFoodFactsProduct>>
{
  private readonly IOpenFoodFactsService _openFoodFactsService;
  private readonly ILogger<SearchProductsQueryHandler> _logger;

  public SearchProductsQueryHandler(
      IOpenFoodFactsService openFoodFactsService,
      ILogger<SearchProductsQueryHandler> logger)
  {
    _openFoodFactsService = openFoodFactsService ?? throw new ArgumentNullException(nameof(openFoodFactsService));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public async Task<IEnumerable<OpenFoodFactsProduct>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ProductName))
    {
      _logger.LogWarning("SearchProductsQuery called with null or empty product name");
      return Enumerable.Empty<OpenFoodFactsProduct>();
    }

    _logger.LogInformation("Processing SearchProductsQuery for product name: {ProductName}, page: {Page}, pageSize: {PageSize}",
        request.ProductName, request.Page, request.PageSize);

    try
    {
      var result = await _openFoodFactsService.SearchProductsByNameAsync(
          request.ProductName,
          request.Page,
          request.PageSize,
          cancellationToken);

      var products = result.ToList();
      _logger.LogInformation("Successfully retrieved {Count} products for search: {ProductName}",
          products.Count, request.ProductName);

      return products;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error processing SearchProductsQuery for product name: {ProductName}", request.ProductName);
      throw;
    }
  }
}