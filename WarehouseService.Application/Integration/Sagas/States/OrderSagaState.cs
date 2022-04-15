using Automatonymous;
using MassTransit.Saga;

namespace WarehouseService.Application.Integration.Sagas.States
{
    public class OrderSagaState : SagaStateMachineInstance, ISagaVersion
    {
        public int Version { get; set; }

        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }
    }
}
