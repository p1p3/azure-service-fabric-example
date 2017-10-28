using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUGO.EventBus.Abstractions;
using Microsoft.Azure.ServiceBus;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusServiceBus;
using Newtonsoft.Json;

namespace IUGO.EventBus.AzureServiceBus
{
    public class EventBusServiceBus : IEventBusSuscriber , IEventBusPublisher
    {
   
        private readonly IEventBusPublisher _publisher;
        private readonly IEventBusSuscriber _suscriber;

        public EventBusServiceBus(IEventBusPublisher publisher, IEventBusSuscriber suscriber)
        {
            _publisher = publisher;
            _suscriber = suscriber;
        }

        public void Publish(IntegrationEvent @event)
        {
           _publisher.Publish(@event);
        }

        public void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _suscriber.SubscribeDynamic<TH>(eventName);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
           _suscriber.Subscribe<T,TH>();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _suscriber.Unsubscribe<T,TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
           _suscriber.UnsubscribeDynamic<TH>(eventName);
        }

        public void Dispose()
        {
            _suscriber.Dispose();
        }
    }
}
