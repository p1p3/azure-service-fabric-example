using System.Threading;
using System.Threading.Tasks;
using IUGO.EventBus.Abstractions;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

namespace IUGO.EventBus.AzureServiceFabric.ServiceListener
{
    public class ServiceBusEventBusListener<T, TH> : ICommunicationListener where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        private IEventBusSuscriber _eventBusSuscriber;

        public ServiceBusEventBusListener(IEventBusSuscriber eventBusSuscriber)
        {
            _eventBusSuscriber = eventBusSuscriber;
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
            _eventBusSuscriber.Subscribe<T, TH>();
            return Task.FromResult(string.Empty);
        }

        private void Stop()
        {
            _eventBusSuscriber.Unsubscribe<T, TH>();
            _eventBusSuscriber.Dispose();
            _eventBusSuscriber = null;
        }
    }
}


