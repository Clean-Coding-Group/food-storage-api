# OpenFoodFacts API Integration with AutoFac and DTOs

This project has been set up with comprehensive DTO (Data Transfer Object) support for the OpenFoodFacts API integration using AutoFac for dependency injection.

## Architecture Overview

### DTOs

- **`OpenFoodFactsProductDto`**: Represents product information with computed properties
- **`OpenFoodFactsApiResponseDto`**: Wrapper for API responses with status information

### AutoFac Configuration

- **AutoFac**: Used for dependency injection instead of built-in .NET DI
- **AutoMapper**: Handles mapping between domain models and DTOs
- **Singleton pattern**: Services are registered as singletons for optimal performance

## Key Features

### 1. Data Transfer Objects (DTOs)

Located in `src/Api/DTOs/OpenFoodFacts/`:

```csharp
public class OpenFoodFactsProductDto
{
    public string Code { get; set; }
    public string? ProductName { get; set; }
    public string? Brands { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? ServingQuantity { get; set; }
    public string? ServingQuantityUnit { get; set; }
    public string? ImageIngredientsUrl { get; set; }

    // Computed Properties
    public List<string> BrandList { get; set; }
    public string ServingSize { get; set; }
    public bool HasImage { get; set; }
    public bool HasIngredientsImage { get; set; }
}
```

### 2. AutoFac Dependency Injection

Configured in `src/Api/DependencyInjection/ApplicationModule.cs`:

- **HttpClient**: Configured with timeout and user agent
- **BaseWebServiceClient**: Registered as singleton
- **OpenFoodFactsService**: Registered as singleton
- **AutoMapper**: Configured with mapping profiles

### 3. AutoMapper Configuration

Profile located in `src/Api/Mapping/OpenFoodFactsMappingProfile.cs`:

```csharp
public class OpenFoodFactsMappingProfile : Profile
{
    public OpenFoodFactsMappingProfile()
    {
        CreateMap<OpenFoodFactsProduct, OpenFoodFactsProductDto>();
        CreateMap<OpenFoodFactsApiResponse, OpenFoodFactsApiResponseDto>();
    }
}
```

### 4. Controller Updates

The `ProductLookupController` has been updated to:

- Use AutoMapper for domain-to-DTO conversion
- Return DTOs instead of domain models
- Maintain proper separation of concerns

## API Endpoints

### Get Product by Barcode

```http
GET /api/ProductLookup/barcode/{barcode}
```

Returns: `OpenFoodFactsApiResponseDto`

### Search Products

```http
GET /api/ProductLookup/search?query=nutella&pageSize=20&page=1
```

Returns: `IEnumerable<OpenFoodFactsProductDto>`

### API Information

```http
GET /api/ProductLookup/info
```

Returns: Basic API information

## Testing

### Unit Tests

- **27 passing tests** covering DTOs, mapping, and controller functionality
- Located in `tests/FoodStorageApi.Api.UnitTests/`
- Covers mapping validation, DTO properties, and controller behavior

### Test Coverage

- DTO property validation
- AutoMapper configuration verification
- Controller input validation
- Error handling scenarios
- Successful response mapping

## Benefits of This Architecture

1. **Separation of Concerns**: DTOs keep API contracts separate from domain models
2. **Version Stability**: API responses remain consistent even if domain models change
3. **Performance**: AutoFac singleton registration optimizes service creation
4. **Testability**: Easy to test mapping and controller logic independently
5. **Maintainability**: Clean architecture with proper dependency injection

## Running the Application

```bash
# Build the solution
dotnet build

# Run unit tests
dotnet test tests/FoodStorageApi.Api.UnitTests

# Start the API
dotnet run --project src/Api/FoodStorageApi.Api.csproj
```

The API will be available at `http://localhost:5000` with Swagger documentation at `http://localhost:5000/swagger`.
