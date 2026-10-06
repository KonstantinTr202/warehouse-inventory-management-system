namespace WarehouseInventory.Contracts;

public record AddStockRequest(
    Guid ItemId,
    Guid WarehouseId,
    decimal Quantity,
    Guid MovementDocumentId
);

public record DeductStockRequest(
    Guid ItemId,
    Guid WarehouseId,
    decimal Quantity,
    Guid MovementDocumentId
);

public record StockUpdateResult(
    bool Success,
    decimal NewBalance,
    string? ErrorCode
);

public interface IStockUpdateService
{
    Task<StockUpdateResult> AddStockAsync(AddStockRequest request, CancellationToken cancellationToken);
    Task<StockUpdateResult> DeductStockAsync(DeductStockRequest request, CancellationToken cancellationToken);
}
