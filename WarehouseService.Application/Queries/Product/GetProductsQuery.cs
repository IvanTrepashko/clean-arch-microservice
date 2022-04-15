using MediatR;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Application.Queries
{
    public class GetProductsQuery : IRequest<GetProductsResponse>
    {
    }

    public class GetProductsResponse
    {
        public IEnumerable<Product> Products { get; init; }

        public GetProductsResponse(IEnumerable<Product> products)
        {
            Products = products;
        }
    }
}
