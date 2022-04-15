using MassTransit;
using Warehouse.Shared.Events;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Entities;
using WarehouseService.Domain.Enums;

namespace WarehouseService.Application.Integration.Consumer
{
    public class OrderDeclinedConsumer : IConsumer<OrderDeclined>
    {
        private readonly IOrderRepository orderRepository;

        public OrderDeclinedConsumer(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<OrderDeclined> context)
        {
            var order = new Order(context.Message.OrderId, context.Message.UserId,
               context.Message.ProductId, context.Message.Count, OrderStatus.Declined);

            await orderRepository.AddAsync(order);
        }
    }
}
