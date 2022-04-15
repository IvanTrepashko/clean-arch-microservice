using WarehouseService.Domain.Entities;
using WarehouseService.Domain.Enums;

namespace WarehouseService.Domain.Abstractions
{
    public interface IProductService
    {
        Task<StockThreshold> GetStockThresholdAsync(Product product);
    }
}
