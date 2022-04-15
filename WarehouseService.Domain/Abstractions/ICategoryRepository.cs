using WarehouseService.Domain.Entities;

namespace WarehouseService.Domain.Abstractions
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);

        Task UpdateAsync(Category category);

        Task DeleteAsync(uint id);

        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category> GetByIdAsync(uint id);
    }
}
