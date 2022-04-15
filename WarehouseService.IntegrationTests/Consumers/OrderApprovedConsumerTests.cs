using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Linq;
using Warehouse.Shared.Events;
using WarehouseService.Application.Integration.Consumer;
using WarehouseService.Domain.Abstractions;
using Xunit;

namespace WarehouseService.IntegrationTests.Consumers
{
    public class OrderApprovedConsumerTests
    {
        [Fact]
        public async void OrderApprovedConsumed_Success()
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            var productRepository = Substitute.For<IProductRepository>();

            var harness = new InMemoryTestHarness();

            var consumer = harness.Consumer(() => new OrderApprovedConsumer(orderRepository, productRepository));

            var eventId = NewId.NextGuid();

            await harness.Start();

            try
            {
                await harness.InputQueueSendEndpoint.Send(new OrderApproved()
                {
                    Count = 1,
                    EventId = eventId,
                    OrderId = 1,
                    ProductId = 1,
                    Timestamp = DateTime.UtcNow,
                    UserId = 1,
                });

                Assert.True(harness.Sent.Select<OrderApproved>().Any());
                Assert.True(consumer.Consumed.Select<OrderApproved>().Any());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
