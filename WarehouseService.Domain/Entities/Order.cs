using WarehouseService.Domain.Enums;

namespace WarehouseService.Domain.Entities
{
    public class Order
    {
        public uint Id { get; init; }

        public uint UserId { get; init; }

        public uint ProductId { get; init; }

        public int Count { get; init; }

        public OrderStatus OrderStatus { get; init; }

        public Order(uint id, uint userId, uint productId, int count, OrderStatus orderStatus)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
            }

            Id = id;
            UserId = userId;
            ProductId = productId;
            Count = count;
            OrderStatus = orderStatus;
        }
    }
}
