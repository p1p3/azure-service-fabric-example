using System;
using System.Text;
using IUGO.EventBus.Abstractions;
using Microsoft.Azure.ServiceBus;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusServiceBus;
using Newtonsoft.Json;

namespace IUGO.EventBus.AzureServiceBus
{
    public class EventBusServiceBusPublisher : IEventBusPublisher
    {
        private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;

        public EventBusServiceBusPublisher(IServiceBusPersisterConnection serviceBusPersisterConnection)
        {
            _serviceBusPersisterConnection = serviceBusPersisterConnection;
        }

        public void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name.Replace(IntegrationEvent.INTEGRATION_EVENT_SUFIX, "");
            var jsonMessage = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new Message
            {
                MessageId = new Guid().ToString(),
                Body = body,
                Label = eventName,
            };

            var topicClient = _serviceBusPersisterConnection.CreateModel();

            topicClient.SendAsync(message)
                .GetAwaiter()
                .GetResult();
        }

        public void Dispose()
        {
            
        }
    }
}
