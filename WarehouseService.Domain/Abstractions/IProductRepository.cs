using WarehouseService.Domain.Entities;

namespace WarehouseService.Domain.Abstractions
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product> GetByIdAsync(uint id);

        Task<IEnumerable<Product>> GetProductsByCategory(uint categoryId);
    }
}
