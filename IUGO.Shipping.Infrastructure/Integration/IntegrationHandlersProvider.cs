using System;
using IUGO.EventBus.Abstractions;
using IUGO.EventBus.AzureServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace IUGO.Shippings.Infrastructure.Integration
{
    public class IntegrationHandlersProvider : IHandlerServiceProvider
    {
        private readonly IServiceProvider _servicesProvider;

        public IntegrationHandlersProvider(IServiceProvider servicesProvider)
        {
            this._servicesProvider = servicesProvider;
        }
        
        public static IHandlerServiceProvider CreateDeafultHandlerResolver()
        {
            IServiceCollection services = new ServiceCollection();
            var provider = services.BuildServiceProvider();
            return new IntegrationHandlersProvider(provider);
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
