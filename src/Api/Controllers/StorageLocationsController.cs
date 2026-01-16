using Microsoft.AspNetCore.Mvc;

namespace FoodStorageApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorageLocationsController : ControllerBase
{
  private readonly ILogger<StorageLocationsController> _logger;

  public StorageLocationsController(ILogger<StorageLocationsController> logger)
  {
    _logger = logger;
  }

  [HttpGet]
  public async Task<IActionResult> GetAllStorageLocations()
  {
    _logger.LogInformation("Getting all storage locations");

    // TODO: Implement actual logic
    var locations = new[]
    {
            new { Id = 1, Name = "Refrigerator", Type = "Refrigerated", Temperature = 4, Description = "Main refrigerator in kitchen" },
            new { Id = 2, Name = "Pantry", Type = "Dry Storage", Temperature = 20, Description = "Pantry for non-perishable items" },
            new { Id = 3, Name = "Freezer", Type = "Frozen", Temperature = -18, Description = "Freezer compartment" }
        };

    return Ok(locations);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetStorageLocation(int id)
  {
    _logger.LogInformation("Getting storage location with ID: {Id}", id);

    // TODO: Implement actual logic
    if (id <= 0)
    {
      return BadRequest("Invalid location ID");
    }

    var location = new { Id = id, Name = "Sample Location", Type = "Sample Type", Temperature = 20, Description = "Sample description" };
    return Ok(location);
  }

  [HttpGet("type/{type}")]
  public async Task<IActionResult> GetLocationsByType(string type)
  {
    _logger.LogInformation("Getting storage locations of type: {Type}", type);

    // TODO: Implement actual logic
    if (string.IsNullOrWhiteSpace(type))
    {
      return BadRequest("Invalid storage type");
    }

    var locations = new[]
    {
            new { Id = 1, Name = "Sample Location", Type = type, Temperature = 20, Description = "Sample description" }
        };

    return Ok(locations);
  }

  [HttpPost]
  public async Task<IActionResult> CreateStorageLocation([FromBody] object location)
  {
    _logger.LogInformation("Creating new storage location");

    // TODO: Implement actual logic
    return CreatedAtAction(nameof(GetStorageLocation), new { id = 1 }, location);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateStorageLocation(int id, [FromBody] object location)
  {
    _logger.LogInformation("Updating storage location with ID: {Id}", id);

    // TODO: Implement actual logic
    if (id <= 0)
    {
      return BadRequest("Invalid location ID");
    }

    return Ok(location);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteStorageLocation(int id)
  {
    _logger.LogInformation("Deleting storage location with ID: {Id}", id);

    // TODO: Implement actual logic
    if (id <= 0)
    {
      return BadRequest("Invalid location ID");
    }

    return NoContent();
  }

  [HttpGet("{id}/capacity")]
  public async Task<IActionResult> GetLocationCapacity(int id)
  {
    _logger.LogInformation("Getting capacity information for location ID: {Id}", id);

    // TODO: Implement actual logic
    if (id <= 0)
    {
      return BadRequest("Invalid location ID");
    }

    var capacity = new { LocationId = id, TotalCapacity = 100, UsedCapacity = 65, AvailableCapacity = 35, Unit = "liters" };
    return Ok(capacity);
  }
}