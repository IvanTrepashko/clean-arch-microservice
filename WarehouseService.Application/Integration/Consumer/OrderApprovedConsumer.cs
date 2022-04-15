using MassTransit;
using Warehouse.Shared.Events;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Entities;
using WarehouseService.Domain.Enums;

namespace WarehouseService.Application.Integration.Consumer
{
    public class OrderApprovedConsumer : IConsumer<OrderApproved>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;

        public OrderApprovedConsumer(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
        }

        public async Task Consume(ConsumeContext<OrderApproved> context)
        {
            var order = new Order(context.Message.OrderId, context.Message.UserId,
                context.Message.ProductId, context.Message.Count, OrderStatus.Approved);
            
            await orderRepository.AddAsync(order);

            var product = await productRepository.GetByIdAsync(context.Message.ProductId);
            product.Stock -= context.Message.Count;
            await productRepository.UpdateAsync(product);
        }
    }
}
