using FoodStorageApi.Domain.Models.OpenFoodFacts;
using MediatR;

namespace FoodStorageApi.Application.Features.OpenFoodFacts.Queries;

/// <summary>
/// Query for searching OpenFoodFacts products by name
/// </summary>
public record SearchProductsQuery(
    string ProductName,
    int Page = 1,
    int PageSize = 20
) : IRequest<IEnumerable<OpenFoodFactsProduct>>;