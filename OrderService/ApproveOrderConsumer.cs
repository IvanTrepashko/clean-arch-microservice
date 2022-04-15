using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Shared.Events;

namespace OrderService
{
    public class ApproveOrderConsumer : IConsumer<OrderSubmitted>
    {
        public async Task Consume(ConsumeContext<OrderSubmitted> context)
        {
            bool approve = context.Message.UserId == 2;

            Console.WriteLine($"Received order for approval: \n OrderId = {context.Message.OrderId}.");
            Console.WriteLine($"Count: {context.Message.Count}");
            Console.WriteLine($"ProductId: {context.Message.ProductId}");
            Console.WriteLine($"UserId: {context.Message.UserId}");
            Console.WriteLine($"Is approved: {approve}");

            if (approve)
            {
                await context.Publish(new OrderApproved()
                {
                    EventId = context.Message.EventId,
                    OrderId = context.Message.OrderId,
                    Count = context.Message.Count,
                    ProductId = context.Message.ProductId,
                    UserId = context.Message.UserId,
                    Timestamp = DateTime.UtcNow
                }, context.CancellationToken);
            }
            else
            {
                await context.Publish(new OrderDeclined()
                {
                    EventId = context.Message.EventId,
                    OrderId = context.Message.OrderId,
                    Count = context.Message.Count,
                    ProductId = context.Message.ProductId,
                    UserId = context.Message.UserId,
                    Timestamp = DateTime.UtcNow
                }, context.CancellationToken);
            }
        }
    }
}
