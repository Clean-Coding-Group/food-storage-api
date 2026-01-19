using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Domain.Models.OpenFoodFacts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FoodStorageApi.Application.Features.OpenFoodFacts.Queries;

/// <summary>
/// Handler for GetProductByBarcodeQuery
/// </summary>
public class GetProductByBarcodeQueryHandler : IRequestHandler<GetProductByBarcodeQuery, OpenFoodFactsApiResponse?>
{
  private readonly IOpenFoodFactsService _openFoodFactsService;
  private readonly ILogger<GetProductByBarcodeQueryHandler> _logger;

  public GetProductByBarcodeQueryHandler(
      IOpenFoodFactsService openFoodFactsService,
      ILogger<GetProductByBarcodeQueryHandler> logger)
  {
    _openFoodFactsService = openFoodFactsService ?? throw new ArgumentNullException(nameof(openFoodFactsService));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public async Task<OpenFoodFactsApiResponse?> Handle(GetProductByBarcodeQuery request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.Barcode))
    {
      _logger.LogWarning("GetProductByBarcodeQuery called with null or empty barcode");
      return null;
    }

    _logger.LogInformation("Processing GetProductByBarcodeQuery for barcode: {Barcode}", request.Barcode);

    try
    {
      var result = await _openFoodFactsService.GetProductByBarcodeAsync(request.Barcode, cancellationToken);

      if (result != null)
      {
        _logger.LogInformation("Successfully retrieved product for barcode: {Barcode}", request.Barcode);
      }
      else
      {
        _logger.LogInformation("No product found for barcode: {Barcode}", request.Barcode);
      }

      return result;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error processing GetProductByBarcodeQuery for barcode: {Barcode}", request.Barcode);
      throw;
    }
  }
}