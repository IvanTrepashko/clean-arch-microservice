using MediatR;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Application.Queries.Handlers
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Order>
    {
        private readonly IOrderRepository orderRepository;

        public GetOrderQueryHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            return await orderRepository.GetByIdAsync(request.OrderId);
        }
    }
}
