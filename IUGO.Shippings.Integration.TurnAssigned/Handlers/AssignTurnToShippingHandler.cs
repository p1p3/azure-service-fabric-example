using System;
using System.Threading.Tasks;
using IUGO.EventBus.Abstractions;
using IUGO.Shippings.Services.Interfaces;
using IUGO.Shippings.Services.Interfaces.Models;
using IUGO.Turns.Services.Interface.Integration;

namespace IUGO.Shippings.Integration.TurnAssigned.Handlers
{
    public class AssignTurnToShippingHandler : IIntegrationEventHandler<TurnAssignedMessageIntegrationEvent>
    {
        private readonly IShippingService _shippingsService;

        public AssignTurnToShippingHandler(IShippingService shippingsService)
        {
            _shippingsService = shippingsService;
        }

        public async Task Handle(TurnAssignedMessageIntegrationEvent @event)
        {
            // TODO contact with the driver servvice or include info in the event..
            var driver = new ShippingDriver()
            {
                Id = @event.DriverId,
                ContactNumber = "TODO :3188430126",
                FullName = "TODO: FullName"
            };

            var vehicle = new ShippingVehicle()
            {
                Id = @event.DriverId,
                DesignationId = @event.VehicleDesignationId
            };

            var shippingTurn = new ShippingTurn()
            {
                Vehicle = vehicle,
                Driver =  driver,
                TurnId = @event.TurnId
            };

            var shippingId = Guid.Parse(@event.ShippingServiceId);

            await _shippingsService.AssignTurn(shippingTurn, shippingId);
        }
    }
}
