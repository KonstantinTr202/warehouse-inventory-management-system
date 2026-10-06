using Microsoft.Extensions.Logging;
using WarehouseInventory.Contracts;

namespace WarehouseInventory.Inventory;

public class StockUpdateService : IStockUpdateService
{
    private readonly ILogger<StockUpdateService> _logger;

    // Имитация базы данных для демонстрации (изначально 10 единиц)
    private decimal _currentBalance = 10.0m;

    public StockUpdateService(ILogger<StockUpdateService> logger)
    {
        _logger = logger;
    }

    public async Task<StockUpdateResult> DeductStockAsync(DeductStockRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Попытка списания {Quantity} ед. товара {ItemId} со склада {WarehouseId}",
            request.Quantity, request.ItemId, request.WarehouseId);

        // Отрицательный сценарий: проверка остатка
        if (_currentBalance < request.Quantity)
        {
            _logger.LogWarning("Недостаточно остатков для товара {ItemId}. Запрошено: {Requested}, Доступно: {Available}",
                request.ItemId, request.Quantity, _currentBalance);

            return new StockUpdateResult(false, _currentBalance, "INSUFFICIENT_STOCK");
        }

        // Успешный сценарий
        _currentBalance -= request.Quantity;
        _logger.LogInformation("Успешное списание. Новый остаток товара {ItemId}: {NewBalance}",
            request.ItemId, _currentBalance);

        return await Task.FromResult(new StockUpdateResult(true, _currentBalance, null));
    }
}