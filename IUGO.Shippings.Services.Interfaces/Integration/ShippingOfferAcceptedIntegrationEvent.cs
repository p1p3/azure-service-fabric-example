using IUGO.EventBus;

namespace IUGO.Shippings.Services.Interfaces.Integration
{
    public class ShippingOfferAcceptedIntegrationEvent : IntegrationEvent
    {
        public string TurnId { get; set; }
        public string ShippingId { get; set; }
    }
}
