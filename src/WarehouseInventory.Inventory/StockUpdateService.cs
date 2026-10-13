using Microsoft.Extensions.Logging;
using WarehouseInventory.Contracts;

namespace WarehouseInventory.Inventory;

public class StockUpdateService : IStockUpdateService
{
    private readonly ILogger<StockUpdateService> _logger;
    private decimal _currentBalance = 10.0m;

    public StockUpdateService(ILogger<StockUpdateService> logger)
    {
        _logger = logger;
    }

    public Task<StockUpdateResult> AddStockAsync(AddStockRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Оприходование: +{Quantity} ед. товара {ItemId} на склад {WarehouseId}",
            request.Quantity, request.ItemId, request.WarehouseId);

        if (request.Quantity <= 0)
        {
            _logger.LogWarning("Некорректное количество: {Quantity}", request.Quantity);
            return Task.FromResult(new StockUpdateResult(false, _currentBalance, "INVALID_QUANTITY"));
        }

        _currentBalance += request.Quantity;

        _logger.LogInformation("Оприходование успешно. Новый остаток: {Balance}", _currentBalance);
        return Task.FromResult(new StockUpdateResult(true, _currentBalance, null));
    }

    public Task<StockUpdateResult> DeductStockAsync(DeductStockRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Комментарий к операции: {request.Comment ?? "без комментария"}");

        _logger.LogInformation("Отгрузка: -{Quantity} ед. товара {ItemId} со склада {WarehouseId}",
            request.Quantity, request.ItemId, request.WarehouseId);

        if (request.Quantity <= 0)
        {
            _logger.LogWarning("Некорректное количество: {Quantity}", request.Quantity);
            return Task.FromResult(new StockUpdateResult(false, _currentBalance, "INVALID_QUANTITY"));
        }

        if (_currentBalance < request.Quantity)
        {
            _logger.LogWarning("Недостаточно остатков. Запрошено: {Requested}, Доступно: {Available}",
                request.Quantity, _currentBalance);

            return Task.FromResult(new StockUpdateResult(false, _currentBalance, "INSUFFICIENT_STOCK"));
        }

        _currentBalance -= request.Quantity;

        _logger.LogInformation("Отгрузка успешна. Новый остаток: {Balance}", _currentBalance);
        return Task.FromResult(new StockUpdateResult(true, _currentBalance, null));
    }
}
