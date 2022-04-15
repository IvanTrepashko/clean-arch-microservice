using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Entities;
using WarehouseService.Domain.Enums;

namespace WarehouseService.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly ICategoryRepository categoryRepository;

        public ProductService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<StockThreshold> GetStockThresholdAsync(Product product)
        {
            var category = await categoryRepository.GetByIdAsync(product.CategoryId);

            if (product.Stock < category.OutOfStockCount)
            {
                return StockThreshold.OutOfStock;
            }
            else if (product.Stock > category.OutOfStockCount && product.Stock < category.LowStockCount)
            {
                return StockThreshold.LowStock;
            }
            else
            {
                return StockThreshold.Available;
            }
        }
    }
}
