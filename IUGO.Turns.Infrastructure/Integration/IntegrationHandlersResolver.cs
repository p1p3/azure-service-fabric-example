using System;
using IUGO.EventBus.Abstractions;
using IUGO.EventBus.AzureServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace IUGO.Turns.Infrastructure.Integration
{
    public class IntegrationHandlersResolver : IServiceResolver
    {
        private readonly IServiceProvider _servicesProvider;

        public IntegrationHandlersResolver(IServiceProvider servicesProvider)
        {
            this._servicesProvider = servicesProvider;
        }
        
        public static IServiceResolver CreateDeafultHandlerResolver()
        {
            IServiceCollection services = new ServiceCollection();
            var provider = services.BuildServiceProvider();
            return new IntegrationHandlersResolver(provider);
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
