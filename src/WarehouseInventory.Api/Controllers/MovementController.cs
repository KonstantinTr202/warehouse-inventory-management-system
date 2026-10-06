using Microsoft.AspNetCore.Mvc;
using WarehouseInventory.Contracts;
using WarehouseInventory.Inventory;

namespace WarehouseInventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovementController : ControllerBase
{
    private readonly IStockUpdateService _stockService;

    public MovementController(IStockUpdateService stockService)
    {
        _stockService = stockService;
    }

    [HttpPost("shipment")]
    public async Task<IActionResult> RegisterShipment([FromBody] DeductStockRequest request)
    {
        var result = await _stockService.DeductStockAsync(request, HttpContext.RequestAborted);

        if (!result.Success)
        {
            return BadRequest(new { message = "Отгрузка невозможна", error = result.ErrorCode, currentBalance = result.NewBalance });
        }

        return Ok(new { message = "Отгрузка успешно проведена", newBalance = result.NewBalance });
    }
}