using Microsoft.AspNetCore.Mvc;

namespace FoodStorageApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoodStorageController : ControllerBase
{
  private readonly ILogger<FoodStorageController> _logger;

  public FoodStorageController(ILogger<FoodStorageController> logger)
  {
    _logger = logger;
  }

  [HttpGet]
  public async Task<IActionResult> GetAllStoredFood()
  {
    _logger.LogInformation("Getting all stored food items");

    // TODO: Implement actual logic
    var storedFood = new[]
    {
            new { Id = 1, FoodId = 1, LocationId = 1, Quantity = 2, Unit = "liters", ExpiryDate = DateTime.Now.AddDays(7) },
            new { Id = 2, FoodId = 2, LocationId = 2, Quantity = 1, Unit = "loaf", ExpiryDate = DateTime.Now.AddDays(3) },
            new { Id = 3, FoodId = 3, LocationId = 1, Quantity = 5, Unit = "kg", ExpiryDate = DateTime.Now.AddYears(2) }
        };

    return Ok(storedFood);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetStoredFood(int id)
  {
    _logger.LogInformation("Getting stored food item with ID: {Id}", id);

    // TODO: Implement actual logic
    if (id <= 0)
    {
      return BadRequest("Invalid storage ID");
    }

    var storedFood = new { Id = id, FoodId = 1, LocationId = 1, Quantity = 2, Unit = "pieces", ExpiryDate = DateTime.Now.AddDays(5) };
    return Ok(storedFood);
  }

  [HttpGet("location/{locationId}")]
  public async Task<IActionResult> GetFoodByLocation(int locationId)
  {
    _logger.LogInformation("Getting stored food items for location ID: {LocationId}", locationId);

    // TODO: Implement actual logic
    if (locationId <= 0)
    {
      return BadRequest("Invalid location ID");
    }

    var storedFood = new[]
    {
            new { Id = 1, FoodId = 1, LocationId = locationId, Quantity = 2, Unit = "liters", ExpiryDate = DateTime.Now.AddDays(7) },
            new { Id = 3, FoodId = 3, LocationId = locationId, Quantity = 5, Unit = "kg", ExpiryDate = DateTime.Now.AddYears(2) }
        };

    return Ok(storedFood);
  }

  [HttpGet("expiring")]
  public async Task<IActionResult> GetExpiringFood([FromQuery] int days = 7)
  {
    _logger.LogInformation("Getting food items expiring within {Days} days", days);

    // TODO: Implement actual logic
    var expiringFood = new[]
    {
            new { Id = 2, FoodId = 2, LocationId = 2, Quantity = 1, Unit = "loaf", ExpiryDate = DateTime.Now.AddDays(3) }
        };

    return Ok(expiringFood);
  }

  [HttpPost]
  public async Task<IActionResult> AddFoodToStorage([FromBody] object storedFood)
  {
    _logger.LogInformation("Adding new food item to storage");

    // TODO: Implement actual logic
    return CreatedAtAction(nameof(GetStoredFood), new { id = 1 }, storedFood);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateStoredFood(int id, [FromBody] object storedFood)
  {
    _logger.LogInformation("Updating stored food item with ID: {Id}", id);

    // TODO: Implement actual logic
    if (id <= 0)
    {
      return BadRequest("Invalid storage ID");
    }

    return Ok(storedFood);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> RemoveFoodFromStorage(int id)
  {
    _logger.LogInformation("Removing food item from storage with ID: {Id}", id);

    // TODO: Implement actual logic
    if (id <= 0)
    {
      return BadRequest("Invalid storage ID");
    }

    return NoContent();
  }
}