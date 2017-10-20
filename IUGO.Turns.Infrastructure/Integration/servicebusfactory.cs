﻿using IUGO.EventBus.Abstractions;
using IUGO.EventBus.AzureServiceBus;

namespace IUGO.Turns.Infrastructure.Integration
{
    public class ServiceBusFactory
    {
        public static IEventBus CreateAzureEventBusInstance(string serviceBusConnectionString, string subscriptionClientName)
        {
            var defaultPersister = new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
            return new EventBusServiceBus(defaultPersister, subscriptionClientName, IntegrationHandlersResolver.CreateDeafultHandlerResolver());
        }
    }
}
