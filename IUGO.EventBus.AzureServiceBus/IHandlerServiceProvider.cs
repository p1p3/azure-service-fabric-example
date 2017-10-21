using System;
using IUGO.EventBus.Abstractions;

namespace IUGO.EventBus.AzureServiceBus
{
    public interface IHandlerServiceProvider
    {
        IIntegrationEventHandler CreateInstance(Type type);
    }
}
