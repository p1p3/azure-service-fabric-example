using System;
using IUGO.EventBus.Abstractions;
using IUGO.EventBus.AzureServiceBus;

namespace IUGO.Shipping.Integration.Test
{
    public class IntegrationHandlersResolver : IServiceResolver
    {
        private readonly IServiceProvider _servicesProvider;

        public IntegrationHandlersResolver(IServiceProvider servicesProvider)
        {
            this._servicesProvider = servicesProvider;
        }

        public T CreateInstance<T>() where T : IIntegrationEventHandler
        {
            return (T)_servicesProvider.GetService(typeof(T));
        }

        public T CreateInstance<T>(T type) where T : IIntegrationEventHandler
        {
            return (T)_servicesProvider.GetService(typeof(T));
        }

        public IIntegrationEventHandler CreateInstance(Type type)
        {
            return _servicesProvider.GetService(type) as IIntegrationEventHandler;
        }
    }
}
