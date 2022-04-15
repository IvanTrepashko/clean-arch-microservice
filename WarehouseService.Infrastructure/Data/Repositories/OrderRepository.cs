using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Entities;
using WarehouseService.Infrastructure.Options;

namespace WarehouseService.Infrastructure.Data.Repositories
{
    public class OrderRepository : MongoDbRepository<Order>, IOrderRepository
    {
        private const string CollectionName = "Order";
        public OrderRepository(IOptions<MongoDbOptions> options)
            : base(options, CollectionName)
        {
        }

        public async Task AddAsync(Order order)
        {
            await Collection.InsertOneAsync(order);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return (await Collection.FindAsync(Builders<Order>.Filter.Empty)).ToEnumerable();
        }

        public async Task<Order> GetByIdAsync(uint id)
        {
            return (await Collection.FindAsync(x => x.Id == id)).FirstOrDefault();
        }

        public async Task<IEnumerable<Order>> GetByProductIdAsync(uint productId)
        {
            return (await Collection.FindAsync(x => x.ProductId == productId)).ToEnumerable();
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(uint userId)
        {
            return (await Collection.FindAsync(x => x.UserId == userId)).ToEnumerable();
        }

        public async Task UpdateAsync(Order order)
        {
            var update = Builders<Order>.Update
                .Set(x => x.OrderStatus, order.OrderStatus)
                .Set(x => x.Count, order.Count)
                .Set(x => x.UserId, order.UserId)
                .Set(x => x.ProductId, order.ProductId);

            await Collection.UpdateOneAsync(x => x.Id == order.Id, update);
        }
    }
}
