using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using WarehouseService.Infrastructure.Abstractions;
using WarehouseService.Infrastructure.Options;

namespace WarehouseService.Infrastructure.Data
{
    public class IdGenerator : IIdGenerator
    {
        private readonly IOptions<MongoDbOptions> options;

        public IdGenerator(IOptions<MongoDbOptions> options)
        {
            this.options = options;
        }

        public async Task<uint> GenerateIdAsync(string collectionName)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var mongoDb = client.GetDatabase(options.Value.DatabaseName);

            var idCollection = mongoDb.GetCollection<IdGeneratorState>("Ids");
            await idCollection.InsertOneAsync(new IdGeneratorState() { CollectionName = collectionName, CurrentId = 1 });
            var update = Builders<IdGeneratorState>.Update.Inc(x => x.CurrentId, 1u);

            await idCollection.FindOneAndUpdateAsync(x => string.Equals(x.CollectionName, collectionName), update);

            return (await idCollection.FindAsync(x => string.Equals(x.CollectionName, collectionName))).FirstOrDefault().CurrentId;
        }
    }
}
