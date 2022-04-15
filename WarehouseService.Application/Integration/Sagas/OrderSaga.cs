using Automatonymous;
using MassTransit;
using Warehouse.Shared.Events;
using WarehouseService.Application.Integration.Sagas.States;

namespace WarehouseService.Application.Integration.Sagas
{
    public class OrderSaga : MassTransitStateMachine<OrderSagaState>
    {
        public State PendingApproval { get; set; }

        public Event<OrderSubmitted> OrderSubmitted { get; set; }

        public Event<OrderDeclined> OrderDeclined { get; set; }

        public Event<OrderApproved> OrderApproved { get; set; }

        public OrderSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderSubmitted, x => x.CorrelateById(x => x.Message.EventId));
            Event(() => OrderApproved, x => x.CorrelateById(x => x.Message.EventId));
            Event(() => OrderDeclined, x => x.CorrelateById(x => x.Message.EventId));

            Initially(
                When(OrderSubmitted)
                .TransitionTo(PendingApproval));

            During(PendingApproval,
                When(OrderApproved)
                .Publish(c => new OrderApproved()
                {
                    EventId = c.Data.EventId,
                    OrderId = c.Data.OrderId,
                    Count = c.Data.Count,
                    ProductId = c.Data.ProductId,
                    UserId = c.Data.UserId,
                    Timestamp = DateTime.Now,
                })
                .TransitionTo(Final),

                When(OrderDeclined)
                .Publish(c => new OrderDeclined()
                {
                    EventId = c.Data.EventId,
                    OrderId = c.Data.OrderId,
                    Count = c.Data.Count,
                    ProductId = c.Data.ProductId,
                    UserId = c.Data.UserId,
                    Timestamp = DateTime.Now,
                })
                .TransitionTo(Final)
            );
        }
    }
}
