using MediatR;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Application.Commands
{
    public class UpdateProductCommand : IRequest
    {
        public Product Product { get; init; }

        public uint Id { get; init; }

        public UpdateProductCommand(Product product, uint id)
        {
            Product = product;
            Id = id;
        }
    }
}
