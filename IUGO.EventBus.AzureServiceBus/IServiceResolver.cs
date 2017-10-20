using System;
using IUGO.EventBus.Abstractions;

namespace IUGO.EventBus.AzureServiceBus
{
    public interface IServiceResolver
    {
        T CreateInstance<T>() where T : IIntegrationEventHandler;
        T CreateInstance<T>(T type) where T : IIntegrationEventHandler;
        IIntegrationEventHandler CreateInstance(Type type);
    }
}
