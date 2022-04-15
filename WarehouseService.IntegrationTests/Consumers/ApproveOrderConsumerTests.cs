using MassTransit;
using MassTransit.Testing;
using NSubstitute;
using OrderService;
using System;
using System.Linq;
using Warehouse.Shared.Events;
using WarehouseService.Application.Integration.Consumer;
using WarehouseService.Domain.Abstractions;
using Xunit;

namespace WarehouseService.IntegrationTests.Consumers
{
    public class ApproveOrderConsumerTests
    {
        [Fact]
        public async void ApproveOrder_OrderApproved()
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            var productRepository = Substitute.For<IProductRepository>();

            var harness = new InMemoryTestHarness();

            var approveOrder = harness.Consumer(() => new ApproveOrderConsumer());
            var orderApproved = harness.Consumer(() => new OrderApprovedConsumer(orderRepository, productRepository));

            var eventId = NewId.NextGuid();

            await harness.Start();

            try
            {
                await harness.InputQueueSendEndpoint.Send(new OrderSubmitted()
                {
                    Count = 1,
                    EventId = eventId,
                    OrderId = 1,
                    ProductId = 1,
                    Timestamp = DateTime.UtcNow,
                    UserId = 2,
                });

                Assert.True(harness.Published.Select<OrderApproved>().Any());
                Assert.True(approveOrder.Consumed.Select<OrderSubmitted>().Any());
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async void ApproveOrder_OrderDeclined()
        {
            var orderRepository = Substitute.For<IOrderRepository>();

            var harness = new InMemoryTestHarness();

            var approveOrder = harness.Consumer(() => new ApproveOrderConsumer());
            var orderDeclined = harness.Consumer(() => new OrderDeclinedConsumer(orderRepository));

            var eventId = NewId.NextGuid();

            await harness.Start();

            try
            {
                await harness.InputQueueSendEndpoint.Send(new OrderSubmitted()
                {
                    Count = 1,
                    EventId = eventId,
                    OrderId = 1,
                    ProductId = 1,
                    Timestamp = DateTime.UtcNow,
                    UserId = 3,
                });

                Assert.True(harness.Published.Select<OrderDeclined>().Any());
                Assert.True(approveOrder.Consumed.Select<OrderSubmitted>().Any());
                Assert.True(orderDeclined.Consumed.Select<OrderDeclined>().Any());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
