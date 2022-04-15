using MediatR;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Application.Queries
{
    public class GetProductQuery : IRequest<Product>
    {
        public uint Id { get; init; }

        public GetProductQuery(uint id)
        {
            Id = id;
        }
    }
}
