using Autofac;
using AutoMapper;
using FoodStorageApi.Api.Mapping;
using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Infrastructure.Services;

namespace FoodStorageApi.Api.DependencyInjection;

/// <summary>
/// AutoFac module for configuring application dependencies
/// </summary>
public class ApplicationModule : Autofac.Module
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
    // Infrastructure services are now registered in Program.cs built-in DI
    // to be accessible by MediatR handlers
  }

  private static void RegisterApplicationServices(ContainerBuilder builder)
  {
    // Register any additional application services here
    // Currently, all application services are interfaces pointing to infrastructure implementations
  }
}