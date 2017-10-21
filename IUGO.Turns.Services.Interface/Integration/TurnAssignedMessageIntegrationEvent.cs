using IUGO.EventBus;

namespace IUGO.Turns.Services.Interface.Integration
{
    public class TurnAssignedMessageIntegrationEvent : IntegrationEvent
    {
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public string ShippingServiceId { get; set; }
    }
}
