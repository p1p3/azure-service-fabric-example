﻿using IUGO.EventBus.Abstractions;
using IUGO.EventBus.AzureServiceBus;

namespace IUGO.Shippings.Infrastructure.Integration
{
    public class ServiceBusFactory
    {
        public static IEventBusPublisher CreateAzureEventBusPublisherInstance(string serviceBusConnectionString)
        {
            var defaultPersister = new DefaultServiceBusPersisterConnection(serviceBusConnectionString);

            return new EventBusServiceBusPublisher(defaultPersister);
        }
    }
}
