using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseService.Domain.Abstractions;

namespace WarehouseService.Application.Queries.Handlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, GetOrdersResponse>
    {
        private readonly IOrderRepository orderRepository;

        public GetOrdersByUserIdQueryHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<GetOrdersResponse> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            return new GetOrdersResponse(await orderRepository.GetByUserIdAsync(request.UserId));
        }
    }
}
