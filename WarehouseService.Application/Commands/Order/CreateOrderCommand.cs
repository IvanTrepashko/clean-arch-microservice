using MediatR;
using WarehouseService.Domain.Enums;

namespace WarehouseService.Application.Commands
{
    public class CreateOrderCommand : IRequest
    {
        public uint UserId { get; init; }

        public uint ProductId { get; init; }

        public int Count { get; init; }

        public uint OrderId { get; init; }

        public OutOfStockMode OutOfStockMode { get; init; }

        public CreateOrderCommand(uint userId, uint productId, int count, uint orderId, OutOfStockMode outOfStockMode)
        {
            UserId = userId;
            ProductId = productId;
            Count = count;
            OrderId = orderId;
            OutOfStockMode = outOfStockMode;
        }
    }
}
