using MediatR;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Application.Queries
{
    public class GetOrdersQuery : IRequest<GetOrdersResponse>
    {
    }

    public class GetOrdersResponse
    {
        public IEnumerable<Order> Orders { get; set; }

        public GetOrdersResponse(IEnumerable<Order> orders)
        {
            Orders = orders;
        }
    }
}
