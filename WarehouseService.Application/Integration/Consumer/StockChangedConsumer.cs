using MassTransit;
using Warehouse.Shared.Events;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Entities;
using WarehouseService.Domain.Enums;
using WarehouseService.Domain.Events;

namespace WarehouseService.Application.Integration.Consumer
{
    public class StockChangedConsumer : IConsumer<StockChanged>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductService productService;
        private readonly IBus bus;

        public StockChangedConsumer(IOrderRepository orderRepository, IProductRepository productRepository, IProductService productService, IBus bus)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
            this.productService = productService;
            this.bus = bus;
        }

        public async Task Consume(ConsumeContext<StockChanged> context)
        {
            var pendingOrder = (await orderRepository.GetByProductIdAsync(context.Message.ProductId)).FirstOrDefault(x => x.OrderStatus == OrderStatus.Pending);

            if (pendingOrder is null)
            {
                return;
            }

            var product = await productRepository.GetByIdAsync(context.Message.ProductId);

            if (product.Stock >= pendingOrder.Count)
            {
                var stockThreshold = await productService.GetStockThresholdAsync(product);

                if (stockThreshold == StockThreshold.Available)
                {
                    await CreateOrderAsync(pendingOrder, product);
                }
                else if (stockThreshold == StockThreshold.LowStock)
                {
                    await bus.Publish(new OrderSubmitted()
                    {
                        EventId = Guid.NewGuid(),
                        OrderId = pendingOrder.Id,
                        Count = pendingOrder.Count,
                        ProductId = pendingOrder.ProductId,
                        UserId = pendingOrder.UserId,
                        Timestamp = DateTime.UtcNow,
                    });
                }
            }
        }

        private async Task CreateOrderAsync(Order pendingOrder, Product product)
        {
            var order = new Order(pendingOrder.Id, pendingOrder.UserId, pendingOrder.ProductId, pendingOrder.Count, OrderStatus.Approved);

            await orderRepository.UpdateAsync(order);

            product.Stock -= pendingOrder.Count;
            await productRepository.UpdateAsync(product);
        }
    }
}
