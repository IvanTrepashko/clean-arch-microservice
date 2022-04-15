using System.Threading.Tasks;

namespace WarehouseService.Infrastructure.Abstractions
{
    public interface IIdGenerator
    {
        Task<uint> GenerateIdAsync(string collectionName);
    }
}
