using WarehouseService.Domain.Entities;

namespace WarehouseService.Domain.Abstractions
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);

        Task UpdateAsync(Order order);

        Task<IEnumerable<Order>> GetByUserIdAsync(uint userId);

        Task<Order> GetByIdAsync(uint id);

        Task<IEnumerable<Order>> GetAllAsync();

        Task<IEnumerable<Order>> GetByProductIdAsync(uint productId);
    }
}
