using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Entities;
using WarehouseService.Infrastructure.Options;

namespace WarehouseService.Infrastructure.Data.Repositories
{
    public class CategoryRepository : MongoDbRepository<Category>, ICategoryRepository
    {
        private const string CollectionName = "Category";

        public CategoryRepository(IOptions<MongoDbOptions> options)
            : base(options, CollectionName)
        {
        }

        public async Task AddAsync(Category category)
        {
            if (category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            await Collection.InsertOneAsync(category);
        }

        public async Task DeleteAsync(uint id)
        {
            await Collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return (await Collection.FindAsync(Builders<Category>.Filter.Empty)).ToEnumerable();
        }

        public async Task<Category> GetByIdAsync(uint id)
        {
            return (await Collection.FindAsync(x => x.Id == id)).FirstOrDefault();
        }

        public async Task UpdateAsync(Category category)
        {
            var update = Builders<Category>.Update
                .Set(x => x.Name, category.Name)
                .Set(x => x.OutOfStockCount, category.OutOfStockCount)
                .Set(x => x.LowStockCount, category.LowStockCount);

            await Collection.UpdateOneAsync(x => x.Id == category.Id, update);
        }
    }
}
