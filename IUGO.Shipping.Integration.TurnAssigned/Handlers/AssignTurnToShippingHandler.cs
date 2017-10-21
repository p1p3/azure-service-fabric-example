using System;
using System.Threading.Tasks;
using IUGO.EventBus.Abstractions;
using IUGO.Turns.Services.Interface.Integration;

namespace IUGO.Shipping.Integration.TurnAssigned.Handlers
{
    public class AssignTurnToShippingHandler : IIntegrationEventHandler<TurnAssignedMessageIntegrationEvent>
    {
        public Task Handle(TurnAssignedMessageIntegrationEvent @event)
        {
            Console.WriteLine("llegue");
            return Task.CompletedTask;
        }
    }
}
