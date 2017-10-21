using System.Threading;
using System.Threading.Tasks;
using IUGO.EventBus.Abstractions;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

namespace IUGO.EventBus.AzureServiceFabric.ServiceListener
{
    public class ServiceBusEventBusListener<T, TH> : ICommunicationListener where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        private IEventBus _eventBus;

        public ServiceBusEventBusListener(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Abort()
        {
            // Close down
            Stop();
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            // Close down
            Stop();
            return Task.FromResult(true);
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            _eventBus.Subscribe<T, TH>();
            return Task.FromResult(string.Empty);
        }

        private void Stop()
        {
            _eventBus.Unsubscribe<T, TH>();
            _eventBus.Dispose();
            _eventBus = null;
        }
    }
}


