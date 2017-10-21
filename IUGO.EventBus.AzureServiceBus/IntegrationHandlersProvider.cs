using System;
using IUGO.EventBus.Abstractions;

namespace IUGO.EventBus.AzureServiceBus
{
    public class IntegrationHandlersProvider : IHandlerServiceProvider
    {
        private readonly IServiceProvider _servicesProvider;

        public IntegrationHandlersProvider(IServiceProvider servicesProvider)
        {
            this._servicesProvider = servicesProvider;
        }

        public IIntegrationEventHandler CreateInstance(Type type)
        {
            return _servicesProvider.GetService(type) as IIntegrationEventHandler;
        }
    }
}
