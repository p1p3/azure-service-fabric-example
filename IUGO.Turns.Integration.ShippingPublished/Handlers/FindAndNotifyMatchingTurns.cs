using System.Threading.Tasks;
using IUGO.EventBus.Abstractions;
using IUGO.Shippings.Services.Interfaces.Integration;

namespace IUGO.Turns.Integration.ShippingPublished.Handlers
{
    public class FindAndNotifyMatchingTurns : IIntegrationEventHandler<ShippingPublishedIntegrationEvent>
    {
        public Task Handle(ShippingPublishedIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
