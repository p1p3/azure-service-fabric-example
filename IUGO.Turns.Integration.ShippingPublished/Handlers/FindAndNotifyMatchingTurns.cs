using System.Threading.Tasks;
using IUGO.EventBus.Abstractions;
using IUGO.Shippings.Services.Interfaces.Integration;
using IUGO.Turns.Services.Interface;

namespace IUGO.Turns.Integration.ShippingPublished.Handlers
{
    public class FindAndNotifyMatchingTurns : IIntegrationEventHandler<ShippingPublishedIntegrationEvent>
    {
        private readonly ITurnService _turnService;

        public FindAndNotifyMatchingTurns(ITurnService turnService)
        {
            _turnService = turnService;
        }

        public async Task Handle(ShippingPublishedIntegrationEvent @event)
        {
            var destinationIds = new[] { @event.ShippingInformation.DestinationId };
            var originIds = new[] { @event.ShippingInformation.OrignId };
            var pickUpDate = @event.ShippingInformation.PickUpDate;
            var vehicleDesignations = @event.ShippingInformation.RequiredVehicleDesignationsIds;

            var turns = await _turnService.FindTurnsBy(destinationIds
               , originIds
               , pickUpDate
               , vehicleDesignations);

        }
    }
}
