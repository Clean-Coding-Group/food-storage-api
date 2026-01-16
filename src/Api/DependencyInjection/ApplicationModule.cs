using Autofac;
using AutoMapper;
using FoodStorageApi.Api.Mapping;
using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Infrastructure.Services;

namespace FoodStorageApi.Api.DependencyInjection;

/// <summary>
/// AutoFac module for configuring application dependencies
/// </summary>
public class ApplicationModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    // Register AutoMapper
    var mapperConfig = new MapperConfiguration(cfg =>
    {
      cfg.AddProfile<OpenFoodFactsMappingProfile>();
    });
    var mapper = mapperConfig.CreateMapper();
    builder.RegisterInstance(mapper).As<IMapper>().SingleInstance();

    // Register Infrastructure services
    RegisterInfrastructureServices(builder);

    // Register Application services
    RegisterApplicationServices(builder);
  }

  private static void RegisterInfrastructureServices(ContainerBuilder builder)
  {
    // Register HTTP client for web service calls
    builder.Register(c =>
    {
      var httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Add("User-Agent", "FoodStorageApi/1.0");
      httpClient.Timeout = TimeSpan.FromSeconds(30);
      return httpClient;
    }).As<HttpClient>().SingleInstance();

    // Register BaseWebServiceClient
    builder.RegisterType<BaseWebServiceClient>()
        .As<IBaseWebServiceClient>()
        .SingleInstance();

    // Register OpenFoodFactsService
    builder.RegisterType<OpenFoodFactsService>()
        .As<IOpenFoodFactsService>()
        .SingleInstance();
  }

  private static void RegisterApplicationServices(ContainerBuilder builder)
  {
    // Register any additional application services here
    // Currently, all application services are interfaces pointing to infrastructure implementations
  }
}