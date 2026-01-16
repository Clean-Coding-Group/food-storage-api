using AutoMapper;
using FoodStorageApi.Api.DTOs.OpenFoodFacts;
using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Domain.Models.OpenFoodFacts;
using Microsoft.AspNetCore.Mvc;

namespace FoodStorageApi.Api.Controllers;

/// <summary>
/// Controller for accessing OpenFoodFacts product information
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FoodResearchController : ControllerBase
{
  private readonly IOpenFoodFactsService _openFoodFactsService;
  private readonly IMapper _mapper;
  private readonly ILogger<FoodResearchController> _logger;

  public FoodResearchController(
      IOpenFoodFactsService openFoodFactsService,
      IMapper mapper,
      ILogger<FoodResearchController> logger)
  {
    _openFoodFactsService = openFoodFactsService ?? throw new ArgumentNullException(nameof(openFoodFactsService));
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  /// <summary>
  /// Gets product information by barcode from OpenFoodFacts
  /// </summary>
  /// <param name="barcode">The product barcode (EAN, UPC, etc.)</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <returns>Product information if found</returns>
  /// <response code="200">Product found and returned successfully</response>
  /// <response code="404">Product not found in OpenFoodFacts database</response>
  /// <response code="400">Invalid barcode format</response>
  /// <response code="500">Internal server error</response>
  [HttpGet("barcode/{barcode}")]
  [ProducesResponseType<OpenFoodFactsApiResponseDto>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<OpenFoodFactsApiResponseDto>> GetProductByBarcode(
      [FromRoute] string barcode,
      CancellationToken cancellationToken = default)
  {
    try
    {
      if (string.IsNullOrWhiteSpace(barcode))
      {
        return BadRequest("Barcode is required and cannot be empty");
      }

      _logger.LogInformation("Received request for product with barcode: {Barcode}", barcode);

      var result = await _openFoodFactsService.GetProductByBarcodeAsync(barcode, cancellationToken);

      if (result == null || !result.IsSuccess)
      {
        _logger.LogInformation("Product not found for barcode: {Barcode}", barcode);
        return NotFound($"Product with barcode '{barcode}' not found");
      }

      var responseDto = _mapper.Map<OpenFoodFactsApiResponseDto>(result);
      return Ok(responseDto);
    }
    catch (ArgumentException ex)
    {
      _logger.LogWarning(ex, "Invalid barcode provided: {Barcode}", barcode);
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error retrieving product for barcode: {Barcode}", barcode);
      return StatusCode(StatusCodes.Status500InternalServerError,
          "An error occurred while retrieving product information");
    }
  }

  /// <summary>
  /// Searches for products by name in OpenFoodFacts database
  /// </summary>
  /// <param name="query">The product name to search for</param>
  /// <param name="pageSize">Number of results per page (1-100, default: 20)</param>
  /// <param name="page">Page number (default: 1)</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <returns>List of matching products</returns>
  /// <response code="200">Search completed successfully</response>
  /// <response code="400">Invalid search parameters</response>
  /// <response code="500">Internal server error</response>
  [HttpGet("search")]
  [ProducesResponseType<IEnumerable<OpenFoodFactsProductDto>>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<IEnumerable<OpenFoodFactsProductDto>>> SearchProducts(
      [FromQuery] string query,
      [FromQuery] int pageSize = 20,
      [FromQuery] int page = 1,
      CancellationToken cancellationToken = default)
  {
    try
    {
      if (string.IsNullOrWhiteSpace(query))
      {
        return BadRequest("Search query is required and cannot be empty");
      }

      if (pageSize <= 0 || pageSize > 100)
      {
        return BadRequest("Page size must be between 1 and 100");
      }

      if (page <= 0)
      {
        return BadRequest("Page number must be greater than 0");
      }

      _logger.LogInformation("Received search request for: {Query}, page: {Page}, size: {PageSize}",
          query, page, pageSize);

      var results = await _openFoodFactsService.SearchProductsByNameAsync(query, pageSize, page, cancellationToken);

      var resultsDto = _mapper.Map<IEnumerable<OpenFoodFactsProductDto>>(results);
      return Ok(resultsDto);
    }
    catch (ArgumentException ex)
    {
      _logger.LogWarning(ex, "Invalid search parameters provided");
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error searching products for query: {Query}", query);
      return StatusCode(StatusCodes.Status500InternalServerError,
          "An error occurred while searching for products");
    }
  }

  /// <summary>
  /// Gets basic information about the OpenFoodFacts integration
  /// </summary>
  /// <returns>API information</returns>
  [HttpGet("info")]
  [ProducesResponseType<object>(StatusCodes.Status200OK)]
  public ActionResult<object> GetApiInfo()
  {
    return Ok(new
    {
      Name = "OpenFoodFacts Integration",
      Description = "Provides access to product information from the OpenFoodFacts database",
      Version = "1.0.0",
      DataSource = "https://world.openfoodfacts.org/",
      SupportedOperations = new[]
        {
                "Get product by barcode",
                "Search products by name"
            },
      RateLimits = "Please respect OpenFoodFacts rate limits",
      Documentation = "https://openfoodfacts.github.io/api-documentation/"
    });
  }
}