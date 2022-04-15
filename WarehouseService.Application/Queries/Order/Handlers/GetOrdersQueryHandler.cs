using MediatR;
using WarehouseService.Domain.Abstractions;

namespace WarehouseService.Application.Queries.Handlers
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, GetOrdersResponse>
    {
        private readonly IOrderRepository orderRepository;

        public GetOrdersQueryHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<GetOrdersResponse> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            return new GetOrdersResponse(await orderRepository.GetAllAsync());
        }
    }
}
