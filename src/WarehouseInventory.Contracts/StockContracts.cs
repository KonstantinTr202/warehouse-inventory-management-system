namespace WarehouseInventory.Contracts;

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
    Task<StockUpdateResult> DeductStockAsync(DeductStockRequest request, CancellationToken cancellationToken);
}