using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Entities;
using WarehouseService.Infrastructure.Options;

namespace WarehouseService.Infrastructure.Data.Repositories
{
    public class ProductRepository : MongoDbRepository<Product>, IProductRepository
    {
        private const string CollectionName = "Product";

        public ProductRepository(IOptions<MongoDbOptions> options)
            : base(options, CollectionName)
        {
        }

        public async Task AddAsync(Product product)
        {
            await Collection.InsertOneAsync(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return (await Collection.FindAsync(Builders<Product>.Filter.Empty)).ToEnumerable();
        }

        public async Task<Product> GetByIdAsync(uint id)
        {
            return (await Collection.FindAsync(x => x.Id == id)).FirstOrDefault();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(uint categoryId)
        {
            return (await Collection.FindAsync(x => x.CategoryId == categoryId)).ToEnumerable();
        }

        public async Task UpdateAsync(Product product)
        {
            var update = Builders<Product>.Update
                .Set(x => x.Name, product.Name)
                .Set(x => x.CategoryId, product.CategoryId)
                .Set(x => x.Stock, product.Stock);

            await Collection.UpdateOneAsync(x => x.Id == product.Id, update);
        }
    }
}
