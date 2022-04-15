using MassTransit;
using MassTransit.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Shared.Events;
using WarehouseService.Application.Integration.Sagas;
using WarehouseService.Application.Integration.Sagas.States;
using Xunit;

namespace WarehouseService.IntegrationTests.Sagas
{
    public class SagaStateMachineTests
    {
        [Fact]
        public async void OrderSubmitted_StateMachineCreated()
        {
            var orderStateMachine = new OrderSaga();

            var harness = new InMemoryTestHarness();
            var saga = harness.StateMachineSaga<OrderSagaState, OrderSaga>(orderStateMachine);

            await harness.Start();

            try
            {
                var eventId = NewId.NextGuid();

                await harness.Bus.Publish(new OrderSubmitted()
                {
                    OrderId = 1,
                    Count = 1,
                    EventId = eventId,
                    ProductId = 1,
                    Timestamp = DateTime.UtcNow,
                    UserId = 1,
                });

                Assert.True(saga.Created.Select(x => x.CorrelationId == eventId).Any());

                var instanceId = await saga.Exists(eventId, x => x.PendingApproval);
                Assert.NotNull(instanceId);
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async void OrderApproved_StateMachineInFinalState()
        {
            var orderStateMachine = new OrderSaga();

            var harness = new InMemoryTestHarness();
            var saga = harness.StateMachineSaga<OrderSagaState, OrderSaga>(orderStateMachine);

            await harness.Start();

            try
            {
                var eventId = NewId.NextGuid();

                await harness.Bus.Publish(new OrderSubmitted()
                {
                    OrderId = 1,
                    Count = 1,
                    EventId = eventId,
                    ProductId = 1,
                    Timestamp = DateTime.UtcNow,
                    UserId = 1,
                });

                Assert.True(saga.Created.Select(x => x.CorrelationId == eventId).Any());

                var instanceId = await saga.Exists(eventId, x => x.PendingApproval);
                Assert.NotNull(instanceId);

                await harness.Bus.Publish(new OrderApproved()
                {
                    OrderId = 1,
                    Count = 1,
                    EventId = eventId,
                    ProductId = 1,
                    Timestamp = DateTime.UtcNow,
                    UserId = 1,
                });

                instanceId = await saga.Exists(eventId, x => x.Final);
                Assert.NotNull(instanceId);
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async Task OrderDeclined_StateMachineInFinalState()
        {
            var orderStateMachine = new OrderSaga();

            var harness = new InMemoryTestHarness();
            var saga = harness.StateMachineSaga<OrderSagaState, OrderSaga>(orderStateMachine);

            await harness.Start();

            try
            {
                var eventId = NewId.NextGuid();

                await harness.Bus.Publish(new OrderSubmitted()
                {
                    OrderId = 1,
                    Count = 1,
                    EventId = eventId,
                    ProductId = 1,
                    Timestamp = DateTime.UtcNow,
                    UserId = 1,
                });

                Assert.True(saga.Created.Select(x => x.CorrelationId == eventId).Any());

                var instanceId = await saga.Exists(eventId, x => x.PendingApproval);
                Assert.NotNull(instanceId);

                await harness.Bus.Publish(new OrderDeclined()
                {
                    OrderId = 1,
                    Count = 1,
                    EventId = eventId,
                    ProductId = 1,
                    Timestamp = DateTime.UtcNow,
                    UserId = 1,
                });

                instanceId = await saga.Exists(eventId, x => x.Final);
                Assert.NotNull(instanceId);
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
