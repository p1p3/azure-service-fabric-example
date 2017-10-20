using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGO.EventBus.Abstractions;
using IUGO.EventBus.AzureServiceBus;
using IUGO.Shipping.Integration.Test.ServiceListener;
using IUGO.Turns.Services.Interface.Integration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace IUGO.Shipping.Integration.Test
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Test : StatelessService
    {
        public Test(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            var eventBus = ServiceConfiguration.ConfigureEventBus(
                "Endpoint=sb://fjaramillo.servicebus.windows.net/;SharedAccessKeyName=manage;SharedAccessKey=mVb/KgmcNz6VMhUf8u+UxXfA3RHusg/eafWcS5KYS18=;EntityPath=turn-assigned"
                , "shipping-turn-assigned");

            yield return new ServiceInstanceListener(context => new ServiceBusEventBusListener<TurnAssignedMessage, TurnAssignedHandler>(eventBus), "StatelessService-ServiceBusQueueListener");
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }

    internal class ServiceConfiguration
    {
        public static IEventBus ConfigureEventBus(string serviceBusConnectionString, string subscriptionClientName)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddTransient<IIntegrationEventHandler<TurnAssignedMessage>, TurnAssignedHandler>();
            var provider = services.BuildServiceProvider();

            var serviceResolver = new IntegrationHandlersResolver(provider);
            var defaultPersister = new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
            return new EventBusServiceBus(defaultPersister, subscriptionClientName, serviceResolver);
        }
    }

    internal class TurnAssignedHandler : IIntegrationEventHandler<TurnAssignedMessage>
    {
        public Task Handle(TurnAssignedMessage @event)
        {
            Console.WriteLine("llegue");
            return Task.CompletedTask;
        }
    }
}
