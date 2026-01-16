using Microsoft.AspNetCore.Mvc;

namespace FoodStorageApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoodInformationController : ControllerBase
{
  private readonly ILogger<FoodInformationController> _logger;

  public FoodInformationController(ILogger<FoodInformationController> logger)
  {
    _logger = logger;
  }

  [HttpGet]
  public async Task<IActionResult> GetAllFoodInformation()
  {
    _logger.LogInformation("Getting all food information");

    // TODO: Implement actual logic
    var foodInfo = new[]
    {
            new { Id = 1, Name = "Milk", Category = "Dairy", Perishable = true },
            new { Id = 2, Name = "Bread", Category = "Grain", Perishable = true },
            new { Id = 3, Name = "Rice", Category = "Grain", Perishable = false }
        };

    return Ok(foodInfo);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetFoodInformation(int id)
  {
    _logger.LogInformation("Getting food information for ID: {Id}", id);

    // TODO: Implement actual logic
    if (id <= 0)
    {
      return BadRequest("Invalid food ID");
    }

    var foodInfo = new { Id = id, Name = "Sample Food", Category = "Sample Category", Perishable = true };
    return Ok(foodInfo);
  }

  [HttpPost]
  public async Task<IActionResult> CreateFoodInformation([FromBody] object foodInformation)
  {
    _logger.LogInformation("Creating new food information");

    // TODO: Implement actual logic
    return CreatedAtAction(nameof(GetFoodInformation), new { id = 1 }, foodInformation);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateFoodInformation(int id, [FromBody] object foodInformation)
  {
    _logger.LogInformation("Updating food information for ID: {Id}", id);

    // TODO: Implement actual logic
    if (id <= 0)
    {
      return BadRequest("Invalid food ID");
    }

    return Ok(foodInformation);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteFoodInformation(int id)
  {
    _logger.LogInformation("Deleting food information for ID: {Id}", id);

    // TODO: Implement actual logic
    if (id <= 0)
    {
      return BadRequest("Invalid food ID");
    }

    return NoContent();
  }
}