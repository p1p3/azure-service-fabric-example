using System;
using System.Threading.Tasks;
using IUGO.EventBus.Abstractions;
using IUGO.Shippings.Services.Interfaces.Integration;
using IUGO.Turns.Services.Interface;

namespace IUGO.Turns.Integration.ShippingOfferAccepted.Handlers
{
    public class AssignShippingToTurn : IIntegrationEventHandler<ShippingOfferAcceptedIntegrationEvent>
    {
        private readonly ITurnService _turnService;

        public AssignShippingToTurn(ITurnService turnService)
        {
            _turnService = turnService;
        }

        public async Task Handle(ShippingOfferAcceptedIntegrationEvent @event)
        {
            var turnId = Guid.Parse(@event.TurnId);
            var shippingId = @event.ShippingId;

            await _turnService.AssignTurnToShippingService(turnId, shippingId);
        }
    }
}
