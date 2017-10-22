using IUGO.EventBus;
using IUGO.Shippings.Services.Interfaces.Models;

namespace IUGO.Shippings.Services.Interfaces.Integration
{
    public class ShippingPublishedIntegrationEvent : IntegrationEvent
    {
        public ShippingOutputModel ShippingInformation { get; set; }
    }
}
