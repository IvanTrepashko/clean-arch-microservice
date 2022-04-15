using MassTransit;
using MassTransit.Testing;
using NSubstitute;
using System;
using System.Linq;
using Warehouse.Shared.Events;
using WarehouseService.Application.Integration.Consumer;
using WarehouseService.Domain.Abstractions;
using Xunit;

namespace WarehouseService.IntegrationTests.Consumers
{
    public class OrderDeclinedConsumerTests
    {
        [Fact]
        public async void OrderDeclinedConsumed_Success()
        {
            var orderRepository = Substitute.For<IOrderRepository>();

            var harness = new InMemoryTestHarness();

            var consumer = harness.Consumer(() => new OrderDeclinedConsumer(orderRepository));

            var eventId = NewId.NextGuid();

            await harness.Start();

            try
            {
                await harness.InputQueueSendEndpoint.Send(new OrderDeclined()
                {
                    Count = 1,
                    EventId = eventId,
                    OrderId = 1,
                    ProductId = 1,
                    Timestamp = DateTime.UtcNow,
                    UserId = 1,
                });

                Assert.True(consumer.Consumed.Select<OrderDeclined>().Any());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
