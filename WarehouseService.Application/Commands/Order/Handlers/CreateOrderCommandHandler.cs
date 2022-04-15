using MassTransit;
using MediatR;
using Warehouse.Shared.Events;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Entities;
using WarehouseService.Domain.Enums;
using WarehouseService.Domain.Exceptions;

namespace WarehouseService.Application.Commands.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductService productService;
        private readonly IOrderRepository orderRepository;
        private readonly IBus bus;

        public CreateOrderCommandHandler(IProductRepository productRepository,
            IProductService productService, IOrderRepository orderRepository, IBus bus)
        {
            this.productRepository = productRepository;
            this.productService = productService;
            this.orderRepository = orderRepository;
            this.bus = bus;
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId);

            if (product is null)
            {
                throw new NotFoundException(request.ProductId);
            }

            var stockThreshold = await productService.GetStockThresholdAsync(product);

            if (stockThreshold == StockThreshold.Available)
            {
                if (request.Count <= product.Stock)
                {
                    await CreateOrderAsync(request, product);
                }
                else
                {
                    await ReserveOutOfStockProductAsync(request);
                }
            }
            else if (stockThreshold == StockThreshold.LowStock && request.Count <= product.Stock)
            {
                await SendOrderForApproval(request, cancellationToken);
            }
            else
            {
                await ReserveOutOfStockProductAsync(request);
            }

            return Unit.Value;
        }

        private async Task ReserveOutOfStockProductAsync(CreateOrderCommand request)
        {
            if (request.OutOfStockMode == OutOfStockMode.ReserveWhenAvailable)
            {
                await CreatePendingOrder(request);
            }
        }

        private async Task CreatePendingOrder(CreateOrderCommand request)
        {
            var order = new Order(request.OrderId, request.UserId, request.ProductId, request.Count, OrderStatus.Pending);

            await orderRepository.AddAsync(order);
        }

        private async Task SendOrderForApproval(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            await bus.Publish(new OrderSubmitted()
            {
                EventId = Guid.NewGuid(),
                OrderId = command.OrderId,
                Count = command.Count,
                ProductId = command.ProductId,
                UserId = command.UserId,
                Timestamp = DateTime.UtcNow,
            }, cancellationToken);
        }

        private async Task CreateOrderAsync(CreateOrderCommand request, Product product)
        {
            var order = new Order(request.OrderId, request.UserId, request.ProductId, request.Count, OrderStatus.Approved);

            await orderRepository.AddAsync(order);

            product.Stock -= request.Count;
            await productRepository.UpdateAsync(product);
        }
    }
}
