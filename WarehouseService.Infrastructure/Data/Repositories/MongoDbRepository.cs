using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WarehouseService.Infrastructure.Options;

namespace WarehouseService.Infrastructure.Data.Repositories
{
    public abstract class MongoDbRepository<T>
    {
        public IMongoCollection<T> Collection { get; init; }

        public MongoDbRepository(IOptions<MongoDbOptions> options, string collectionName)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var mongoDb = client.GetDatabase(options.Value.DatabaseName);

            Collection = mongoDb.GetCollection<T>(collectionName);
        }
    }
}
