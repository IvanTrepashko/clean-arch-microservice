using MongoDB.Bson;

namespace WarehouseService.Infrastructure.Data
{
    internal class IdGeneratorState
    {
        public ObjectId Id { get; set; }

        public string CollectionName { get; set; }

        public uint CurrentId { get; set; }
    }
}
