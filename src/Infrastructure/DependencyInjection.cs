using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FoodStorageApi.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services)
  {
    // Register HttpClient for BaseWebServiceClient
    services.AddHttpClient<IBaseWebServiceClient, BaseWebServiceClient>(client =>
    {
      client.Timeout = TimeSpan.FromSeconds(30);
      client.DefaultRequestHeaders.UserAgent.ParseAdd("FoodStorageApi/1.0");
    });

    // Register OpenFoodFacts service
    services.AddScoped<IOpenFoodFactsService, OpenFoodFactsService>();

    // Register other infrastructure services here

    return services;
  }
}