using IUGO.EventBus.Abstractions;
using IUGO.EventBus.AzureServiceBus;

namespace IUGO.Turns.Infrastructure.Integration
{
    public class ServiceBusFactory
    {
        public static IEventBusPublisher CreateAzureEventBusInstance(string serviceBusConnectionString, string subscriptionClientName)
        {
            var defaultPersister = new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
            return new EventBusServiceBusPublisher(defaultPersister);
        }
    }
}
