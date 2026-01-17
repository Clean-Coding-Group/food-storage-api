using FoodStorageApi.Domain.Models.OpenFoodFacts;
using MediatR;

namespace FoodStorageApi.Application.Features.OpenFoodFacts.Queries;

/// <summary>
/// Query for getting OpenFoodFacts product by barcode
/// </summary>
public record GetProductByBarcodeQuery(string Barcode) : IRequest<OpenFoodFactsApiResponse?>;