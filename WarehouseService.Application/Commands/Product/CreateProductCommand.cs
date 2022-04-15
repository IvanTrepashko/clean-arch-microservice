using MediatR;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Application.Commands
{
    public class CreateProductCommand : IRequest
    {
        public Product Product { get; init; }

        public CreateProductCommand(Product product)
        {
            Product = product;
        }
    }
}
