using Microsoft.Extensions.Logging.Abstractions;
using WarehouseInventory.Contracts;
using WarehouseInventory.Inventory;

namespace WarehouseInventory.UnitTests;

public class StockUpdateServiceTests
{
    private static readonly Guid ItemId = Guid.Parse("416f13ce-c185-4cf2-8a3f-30c1d99e5cc2");
    private static readonly Guid WarehouseId = Guid.Parse("3dc3a71d-9fd6-4e02-89fb-8c12199d1c83");
    private static StockUpdateService CreateService() => new(NullLogger<StockUpdateService>.Instance);
    private static AddStockRequest Receipt(decimal quantity) => new(ItemId, WarehouseId, quantity, Guid.NewGuid());
    private static DeductStockRequest Shipment(decimal quantity) => new(ItemId, WarehouseId, quantity, Guid.NewGuid(), null);

    [Fact]
    public async Task AddStock_ValidQuantity_IncreasesBalance()
    {
        var service = CreateService();
        var result = await service.AddStockAsync(Receipt(20), CancellationToken.None);
        Assert.True(result.Success);
        Assert.Equal(30m, result.NewBalance);
        Assert.Null(result.ErrorCode);
    }

    [Fact]
    public async Task DeductStock_InsufficientStock_ReturnsError()
    {
        var service = CreateService();
        var result = await service.DeductStockAsync(Shipment(11), CancellationToken.None);
        Assert.False(result.Success);
        Assert.Equal("INSUFFICIENT_STOCK", result.ErrorCode);
        Assert.Equal(10m, result.NewBalance);
    }

    [Fact]
    public async Task ReceiptThenShipment_CalculatesBalance()
    {
        var service = CreateService();
        await service.AddStockAsync(Receipt(5), CancellationToken.None);
        var result = await service.DeductStockAsync(Shipment(3), CancellationToken.None);
        Assert.True(result.Success);
        Assert.Equal(12m, result.NewBalance);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidQuantity_IsRejectedWithoutChangingBalance(int quantity)
    {
        var service = CreateService();
        var receipt = await service.AddStockAsync(Receipt(quantity), CancellationToken.None);
        var shipment = await service.DeductStockAsync(Shipment(quantity), CancellationToken.None);
        Assert.Equal("INVALID_QUANTITY", receipt.ErrorCode);
        Assert.Equal("INVALID_QUANTITY", shipment.ErrorCode);
        Assert.Equal(10m, receipt.NewBalance);
        Assert.Equal(10m, shipment.NewBalance);
    }

    [Fact]
    public async Task NullRequest_ThrowsArgumentNullException()
    {
        var service = CreateService();
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddStockAsync(null!, CancellationToken.None));
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.DeductStockAsync(null!, CancellationToken.None));
    }

    [Fact]
    public async Task CancelledRequest_DoesNotChangeBalance()
    {
        var service = CreateService();
        using var source = new CancellationTokenSource();
        source.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => service.AddStockAsync(Receipt(20), source.Token));
        var result = await service.DeductStockAsync(Shipment(1), CancellationToken.None);
        Assert.Equal(9m, result.NewBalance);
    }

    [Fact]
    public async Task ParallelShipments_NeverDeductMoreThanAvailable()
    {
        var service = CreateService();
        var tasks = Enumerable.Range(0, 30).Select(_ => Task.Run(() => service.DeductStockAsync(Shipment(1), CancellationToken.None)));
        var results = await Task.WhenAll(tasks);
        Assert.Equal(10, results.Count(result => result.Success));
        Assert.Equal(20, results.Count(result => result.ErrorCode == "INSUFFICIENT_STOCK"));
        Assert.All(results, result => Assert.InRange(result.NewBalance, 0m, 10m));
    }
}
