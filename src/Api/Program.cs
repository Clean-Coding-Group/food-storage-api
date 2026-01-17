using Autofac;
using Autofac.Extensions.DependencyInjection;
using FoodStorageApi.Api.DependencyInjection;
using FoodStorageApi.Application.Features.OpenFoodFacts.Queries;
using FoodStorageApi.Application.Common.Interfaces;
using FoodStorageApi.Infrastructure.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to built-in DI container that MediatR handlers will need
builder.Services.AddHttpClient<IOpenFoodFactsService, OpenFoodFactsService>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "FoodStorageApi/1.0");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddSingleton<IBaseWebServiceClient, BaseWebServiceClient>();
builder.Services.AddSingleton<IOpenFoodFactsService, OpenFoodFactsService>();

// Add MediatR to the built-in DI container
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetProductByBarcodeQuery>();
});

// Configure AutoFac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<ApplicationModule>();
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
